using DockerPull.Extensions;
using DockerPull.Model;

namespace DockerPull.Function;

public static class LayerCreator
{
    public static IEnumerable<ManifestLayer> CreateLayerInfo(this ManifestResponse response)
    {
        var layerId = "";
        int i = 0, count = response.Layers.Length;
        foreach (var layer in response.Layers)
        {
            var parentId = layerId;
            layerId = $"{parentId}\n{layer.Digest}\n".Hash256();
            yield return new ManifestLayer
            {
                Digest = layer.Digest,
                LayerId = layerId,
                ParentId = parentId,
                Index = i
            };
            i++;
        }
    }
}