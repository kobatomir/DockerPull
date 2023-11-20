using System.Formats.Tar;
using System.IO.Compression;

namespace DockerPull.Extensions;

/// <summary>
/// 压缩解压
/// </summary>
public static class Compress
{
    /// <summary>
    ///  单文件解压缩
    /// </summary>
    /// <param name="compress">压缩文件</param>
    /// <param name="target">目标文件名</param>
    public static void Decompress(string compress, string target)
    {
        using var compressedFileStream = File.Open(compress, FileMode.Open);
        using var outputFileStream = File.Create(target);
        using var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
        decompressor.CopyTo(outputFileStream);
    }

    /// <summary>
    /// 压缩文件夹
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="file"></param>
    public static void CompressToTar(string folder, string file) 
        =>TarFile.CreateFromDirectory(folder,file,false);
}