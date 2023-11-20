using DockerPull.Server;

try
{
    if (args.Length > 0)
    {
        await new Worker(args[0]).Start();
    }
    else
    {
        Console.WriteLine("命令行请使用  DockerPull [registry/][repository/]image[:tag|@digest]");
        Console.WriteLine("已启动交互模式，请输入请求");
        var stand = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(stand))return ;
        await new Worker(stand).Start();
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}