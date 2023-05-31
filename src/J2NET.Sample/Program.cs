namespace J2NET.Sample;

internal static class Program
{
    public static void Main(string[] args)
    {
        var process = JavaRuntime.ExecuteJar("../../../Sample/Sample.jar");
        process.WaitForExit();
    }
}