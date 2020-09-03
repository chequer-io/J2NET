namespace J2NET.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = JavaRuntime.Execute("../../../Sample.jar");
            process.WaitForExit();
        }
    }
}
