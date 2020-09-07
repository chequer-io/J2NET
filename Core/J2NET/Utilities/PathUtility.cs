using System;
using System.IO;
using System.Runtime.InteropServices;

namespace J2NET.Utilities
{
    public static class PathUtility
    {
        public static string GetRuntimePath()
        {
            string platform = null;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                switch (RuntimeInformation.OSArchitecture)
                {
                    case Architecture.X64:
                        platform = "win-x64";
                        break;

                    case Architecture.X86:
                        platform = "win-x86";
                        break;
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                platform = "linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platform = "mac";
            }

            if (string.IsNullOrEmpty(platform))
                throw new PlatformNotSupportedException();

            return Path.Combine("runtimes", platform, "bin", "java");
        }
    }
}
