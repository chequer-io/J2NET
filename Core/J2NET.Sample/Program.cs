namespace J2NET.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = JavaRuntime.Execute("../../../Sample/Sample.jar");
            process.WaitForExit();
        }
    }
}
