using System.Security.Cryptography;
using System.Text;

namespace DockerPull.Extensions;

/// <summary>
/// Hash计算扩展
/// </summary>
public static class HashExtension
{
    /// <summary>
    /// Sha256计算
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string Hash256(this string source)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(source));
        var builder = new StringBuilder();
        foreach (var b in hash) builder.Append(b.ToString("X2").ToLower());
        return builder.ToString();
    }
    
}