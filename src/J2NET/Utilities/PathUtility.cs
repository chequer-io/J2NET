using System;
using System.IO;
using System.Runtime.InteropServices;

namespace J2NET.Utilities
{
    public static class PathUtility
    {
        public static string GetRuntimePath()
        {
            var directory = Path.GetDirectoryName(typeof(PathUtility).Assembly.Location) ?? throw new DirectoryNotFoundException();
            var runtimeIdentifier = GetRuntimeIdentifier();
            return Path.Combine(directory, "runtimes", runtimeIdentifier, "openjre", "bin", "java");
        }

        public static string GetRuntimeIdentifier()
        {
            var arch = RuntimeInformation.ProcessArchitecture;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return arch switch
                {
                    Architecture.X64 => "win-x64",
                    Architecture.X86 => "win-x86",
                    _ => throw new PlatformNotSupportedException($"Windows {arch}")
                };
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return arch switch
                {
                    Architecture.X64 => "osx-x64",
                    Architecture.Arm64 => "osx-arm64",
                    _ => throw new PlatformNotSupportedException($"macOS {arch}")
                };
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (IsAlpineLinux())
                {
                    return arch switch
                    {
                        Architecture.X64 => "linux-musl-x64",
                        Architecture.Arm64 => "linux-musl-arm64",
                        _ => throw new PlatformNotSupportedException($"Alpine Linux {arch}")
                    };
                }

                return arch switch
                {
                    Architecture.X64 => "linux-x64",
                    Architecture.Arm64 => "linux-arm64",
                    _ => throw new PlatformNotSupportedException($"Linux {arch}")
                };
            }

            throw new PlatformNotSupportedException(RuntimeInformation.OSDescription);
        }

        // FIY: https://github.com/dotnet/runtime/blob/5aa9035fc8d45c2a5a51f62e01aff14ad2600c5a/src/libraries/Common/tests/TestUtilities/System/PlatformDetection.Unix.cs#LL232C35-L232C39
        private static bool IsAlpineLinux()
        {
            if (!File.Exists("/etc/os-release"))
                return false;

            foreach (ReadOnlySpan<char> line in File.ReadLines("/etc/os-release"))
            {
                if (!line.StartsWith("ID=", StringComparison.Ordinal))
                    continue;

                ReadOnlySpan<char> id = line[3..].Trim('"').Trim('\'');

                if (id.Equals("alpine", StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
