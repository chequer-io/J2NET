namespace J2NET.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = JavaRuntime.ExecuteJar("../../../Sample/Sample.jar");
            process.WaitForExit();
        }
    }
}
