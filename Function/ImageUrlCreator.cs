using DockerPull.Model;

namespace DockerPull.Function;

public static class ImageUrlCreator
{
    /// <summary>
    ///  身份授权请求
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public static string AuthRequest(this ImageInfo image) =>
        $"{image.Auth}?service={image.Service}&scope=repository:{image.Repository}:pull";

    /// <summary>
    /// 根Manifest请求
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public static string ManifestRequest(this ImageInfo image) =>
        $"https://{image.Registry}/v2/{image.Repository}/manifests/{image.Tag}";

    /// <summary>
    /// Blobs数据请求
    /// </summary>
    /// <param name="image"></param>
    /// <param name="digest"></param>
    /// <returns></returns>
    public static string BlobsRequest(this ImageInfo image, string digest) =>
        $"https://{image.Registry}/v2/{image.Repository}/blobs/{digest}";
}