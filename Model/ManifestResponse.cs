using System.Text.Json.Serialization;

namespace DockerPull.Model;

public class ManifestResponse
{
    [JsonPropertyName("config")]
    public ManifestConfig? Config { set; get; }

    [JsonPropertyName("layers")] 
    public ManifestConfig[] Layers { set; get; } = Array.Empty<ManifestConfig>();
}

public class ManifestConfig
{
    [JsonPropertyName("digest")]
    public string Digest { set; get; } = string.Empty;
    
}