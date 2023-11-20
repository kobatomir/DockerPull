using System.Text.Json.Serialization;

namespace DockerPull.Model;

/// <summary>
/// 身份验证Token
/// </summary>
public record AccessToken
{
    [JsonPropertyName("token")]
    public string? Token { get; init; }
}