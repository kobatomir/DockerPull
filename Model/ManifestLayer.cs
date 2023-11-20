namespace DockerPull.Model;

public class ManifestLayer
{
    public string Digest { init; get; } = string.Empty;

    public string LayerId { init; get; } = string.Empty;

    public string ParentId { set; get; } = string.Empty;
    
    public int Index { set; get; }
}