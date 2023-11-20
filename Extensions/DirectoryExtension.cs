using DockerPull.Model;

namespace DockerPull.Extensions;

public static class DirectoryExtension
{
    /// <summary>
    /// 创建Tmp文件夹
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    private static string CreateTempFolder(this string temp)
    {
        var imagePath = Path.Combine(Environment.CurrentDirectory,"Working",temp);
        Directory.CreateDirectory(imagePath);
        return imagePath;
    }

    /// <summary>
    /// 创建Temp文件夹
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static string CreateTempFolder(this ImageInfo info)=>$"temp_{info.Image}_{info.Tag}".CreateTempFolder();

    /// <summary>
    /// 写文件
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="folder"></param>
    /// <param name="file">文件名</param>
    /// <param name="value">内容</param>
    public static void WriteFileInLayer(this ManifestLayer layer,string folder,string file,string value)
    {
        var sub = Path.Combine(folder, layer.LayerId);
        Directory.CreateDirectory(sub);
        File.WriteAllText(Path.Combine(sub, file), value);
    }
    
}