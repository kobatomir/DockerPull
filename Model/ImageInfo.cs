namespace DockerPull.Model;

public class ImageInfo
{
    /// <summary>
    /// Registry 地址
    /// </summary>
    public string Registry { set; get; } = AddressConst.Registry;
    
    /// <summary>
    /// 服务地址
    /// </summary>
    public string Service { set; get; } = AddressConst.Service;
    

    /// <summary>
    /// 标签Tag
    /// </summary>
    public string Tag { init; get; } = NameConst.Latest;

    /// <summary>
    /// 镜像名称
    /// </summary>
    public string Image { init; get; } = string.Empty;

    /// <summary>
    /// 仓库
    /// </summary>
    public string Library { set; get; } = NameConst.Library;

    /// <summary>
    /// Repository
    /// </summary>
    public string Repository => Library + "/" + Image;

    /// <summary>
    /// 认证中心
    /// </summary>
    public string Auth { set; get; } = AddressConst.AuthUrl;

    /// <summary>
    /// 编译
    /// </summary>
    public bool Combine { set; get; } = false;
}