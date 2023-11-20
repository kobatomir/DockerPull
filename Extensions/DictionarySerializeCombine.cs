using DockerPull.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DockerPull.Extensions
{
    internal static class DictionarySerializeCombine
    {
        public static string Serialize(this Dictionary<string, object> dict)
        {
            var item = dict.Select(item => $"\"{item.Key}\": {(item.Value.GetType() == typeof(JsonElement) ? JsonSerializer.Serialize(item.Value, DictionaryStringContext.Default.JsonElement) : $"\"{item.Value}\"")}");
            return $"{{ {string.Join(",", item)} }}";
        }

        public static string ManifestContentSerialize(this ManifestContent[] main) => $"[{string.Join(",", main.Select(s => JsonSerializer.Serialize(s, ManifestContentContext.Default.ManifestContent)))}]";
    }
}
