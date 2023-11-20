using System.Text.Json;

namespace DockerPull.Extensions;

public static class JsonExtension
{
    public static Dictionary<string, object> ToDictionary(this string json)=>JsonSerializer.Deserialize(json, DictionaryContext.Default.DictionaryStringObject) ??[];
}