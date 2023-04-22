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
                platform = RuntimeInformation.OSArchitecture switch
                {
                    Architecture.X64 => "win-x64",
                    Architecture.X86 => "win-x86",
                    _ => null
                };
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                platform = RuntimeInformation.OSArchitecture switch
                {
                    Architecture.X64 => "linux-x64",
                    Architecture.Arm64 => "linux-arm64",
                    _ => null
                };
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
