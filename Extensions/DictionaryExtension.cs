namespace DockerPull.Extensions;

public  static class DictionaryExtension
{
    public static void RemoveKey(this Dictionary<string, object> dictionary)
    {
        dictionary.Remove("history");
        dictionary.Remove("rootfs");
        dictionary.Remove("rootfS");
    }
}