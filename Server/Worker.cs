using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using DockerPull.Extensions;
using DockerPull.Function;
using DockerPull.Model;

namespace DockerPull.Server;

public class Worker
{
    private readonly HttpClient _client = new HttpClient();

    private readonly ImageInfo _image;

    private ManifestContent[]? _content;

    public Worker(string info) => _image = ImageInfoCreator.Generator(info);



    /// <summary>
    /// 临时文件夹
    /// </summary>
    private string _folder = string.Empty;

    /// <summary>
    /// 获取认证中心
    /// </summary>
    private async ValueTask GetAuthUrl()
    {
        var response = await _client.GetAsync($"https://{_image.Registry}/v2");
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var field = response.Headers.WwwAuthenticate.FirstOrDefault()?.Parameter
                ?.Split("\"", StringSplitOptions.RemoveEmptyEntries);
            _image.Auth = field?[1] ?? AddressConst.AuthUrl;
            if (field?.Length > 3) _image.Service = field[3];
        }
    }

    /// <summary>
    /// 身份授权
    /// </summary>
    /// <returns></returns>
    private async ValueTask<string> Auth()
    {
        _client.DefaultRequestHeaders.Authorization = null;
        var da = await _client.GetStringAsync(_image.AuthRequest());
        var token = JsonSerializer.Deserialize(da, AccessTokenContext.Default.AccessToken);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(NameConst.Bearer, token?.Token);
        return token?.Token ?? string.Empty;
    }

    private async ValueTask<ManifestResponse?> GetManifest()
    {
        var token = await Auth();
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(_image.ManifestRequest()),
            Method = HttpMethod.Get,
        };
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(HeaderValues.ManifestV2));
        request.Headers.Authorization = new AuthenticationHeaderValue(NameConst.Bearer, token);
        var resp = await _client.SendAsync(request);
        if (resp.StatusCode != HttpStatusCode.OK)
        {
            if (resp.StatusCode == HttpStatusCode.NotFound)
                Console.WriteLine("输入的标签信息错误");
            Environment.Exit(0);
        }

        var response = await resp.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize(response, ManifestResponseContext.Default.ManifestResponse);
    }



    public async ValueTask Start()
    {
        await GetAuthUrl();
        var manifest = await GetManifest();
        if (manifest == null) return;
        _folder = _image.CreateTempFolder();
        var digest = await _client.GetStringAsync(_image.BlobsRequest(manifest.Config!.Digest));
        await File.WriteAllTextAsync(Path.Combine(_folder, manifest.Config.Digest[7..] + ".json"), digest);

        var layers = manifest.CreateLayerInfo().ToArray();
        _content = _image.CreateContent(manifest);
        await Parallel.ForEachAsync(layers, async (layer, token) =>
        {
            Console.WriteLine("Read " + layer.Digest);
            layer.WriteFileInLayer(_folder, NameConst.Version, NameConst.VersionValue);
            //写入层
            var response = await _client.GetStreamAsync(_image.BlobsRequest(layer.Digest), token);
            var so = Path.Combine(_folder, layer.LayerId, "layer_gzip.tar");
            await using var file = File.OpenWrite(so);
            await response.CopyToAsync(file, token);
            await file.DisposeAsync();
            //解压文件
            var target = Path.Combine(_folder, layer.LayerId, "layer.tar");
            Compress.Decompress(so, target);
            //删除层
            File.Delete(so);
            //写配置
            _content![0].Layers[layer.Index] = layer.LayerId + "/layer.tar";
            var last = layer.Index == manifest.Layers.Length - 1;
            var json = last ? digest.ToDictionary() : ManifestContentCreator.CreateEmpty();
            json["id"] = layer.LayerId;
            if (!string.IsNullOrWhiteSpace(layer.ParentId)) json["parent"] = layer.LayerId;
            if (last) json.RemoveKey();
            //写文件
            var key = DictionarySerializeCombine.Serialize(json);
            await File.WriteAllTextAsync(Path.Combine(_folder, layer.LayerId, "json"), key, token);
            Console.WriteLine("Read End " + layer.Digest);
        });

        // 写Manifest文件
        var manifestInfo = _content.ManifestContentSerialize();
        await File.WriteAllTextAsync(Path.Combine(_folder, "manifest.json"), manifestInfo);

        await File.WriteAllTextAsync(Path.Combine(_folder, "repositories"), _image.RepositoryInfo(layers.Last().LayerId));

        var target = Path.Combine(Environment.CurrentDirectory, $"Images");
        Directory.CreateDirectory(target);
        var name = Path.Combine(target, _image.Image + "_" + _image.Tag + ".tar");
        Compress.CompressToTar(_folder, name);

        Directory.Delete(_folder,true);

        Console.WriteLine("打包已完成");
    }
}