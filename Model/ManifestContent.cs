namespace DockerPull.Model;

/// <summary>
/// Manifest 文件内容
/// </summary>
public class ManifestContent
{
    public string Config { set; get; } = string.Empty;

    public string[] RepoTags { set; get; } = Array.Empty<string>();

    public string[] Layers { set; get; } = Array.Empty<string>();
}