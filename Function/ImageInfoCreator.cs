using System.Text.Json;
using DockerPull.Extensions;
using DockerPull.Model;

namespace DockerPull.Function;

public static class ImageInfoCreator
{
    /// <summary>
    /// 根据请求信息获取镜像数据
    /// </summary>
    /// <param name="signal"></param>
    /// <returns></returns>
    public  static ImageInfo Generator(string signal)
    {
        var tags= signal.Split('/');
        var config=  tags[^1].Contains('@') ? tags[^1].Split("@") :
            tags[^1].Contains(':') ? tags[^1].Split(":") :
            new[] { tags[^1], NameConst.Latest };
        var info = new ImageInfo() { Image = config[0], Tag = config[1] };
        if (tags.Length > 1 && (tags[0].Contains('.') || tags[0].Contains(':'))) info.Registry = tags[0];
        if (tags.Length > 2) info.Library = string.Join("/", tags[1..^1]);
        info.Combine = tags.Length > 1;
        return info;
    }

    public static string RepositoryInfo(this ImageInfo info,string layerId)
    {
        var dict=new Dictionary<string,object>();
        var key = info.Combine ? info.Registry + "/" + info.Repository : info.Image; 
        dict[key]= new { tag= layerId };
        return dict.Serialize();
    }
}