using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace J2NET
{
    public static class JavaRuntime
    {
        public static Process Execute(string value)
        {
            string java = null;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                switch (RuntimeInformation.OSArchitecture)
                {
                    case Architecture.X64:
                        java = "win-64";
                        break;

                    case Architecture.X86:
                        java = "win-32";
                        break;
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                java = "linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                java = "mac";
            }

            return Process.Start(Path.Combine("runtime", java, "bin", "java"), $"-jar {value}");
        }
    }
}
