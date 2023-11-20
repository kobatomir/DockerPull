using DockerPull.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DockerPull.Extensions
{

    [JsonSerializable(typeof(AccessToken))]
    public partial class AccessTokenContext : JsonSerializerContext { }

    [JsonSerializable(typeof(ManifestResponse))]
    public partial class ManifestResponseContext : JsonSerializerContext { }

    [JsonSerializable(typeof(Dictionary<string, object>))]
    public partial class DictionaryContext : JsonSerializerContext { }

    [JsonSerializable(typeof(ManifestContent[]))]
    public partial class ManifestContentContext : JsonSerializerContext { }

    [JsonSerializable(typeof(JsonElement))]
    public partial class DictionaryStringContext : JsonSerializerContext { }

}
