using System.Text.Json;
using DockerPull.Extensions;
using DockerPull.Model;

namespace DockerPull.Function;

public static class ManifestContentCreator
{
    public static ManifestContent[] CreateContent(this ImageInfo info, ManifestResponse option) => new ManifestContent[]
    {
        new()
        {
            Config = option.Config!.Digest[7..] + ".json",
            RepoTags = new[]
                { info.Combine ? info.Registry + "/" + info.Repository + ":" + info.Tag : info.Image + ":" + info.Tag },
            Layers = new string[option.Layers.Length]
        }
    };

    public static Dictionary<string, object> CreateEmpty()
        => JsonSerializer.Deserialize(
            """
            {"created":"1970-01-01T00:00:00Z","container_config":{"Hostname":"","Domainname":"","User":"","AttachStdin":false,
            	"AttachStdout":false,"AttachStderr":false,"Tty":false,"OpenStdin":false, "StdinOnce":false,"Env":null,"Cmd":null,"Image":"",
            	"Volumes":null,"WorkingDir":"","Entrypoint":null,"OnBuild":null,"Labels":null}}
            """, DictionaryContext.Default.DictionaryStringObject) ?? [];
}