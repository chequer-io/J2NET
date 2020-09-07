using System.Diagnostics;
using System.IO;
using J2NET.Utilities;

namespace J2NET
{
    public static class JavaRuntime
    {
        public static Process Execute(string value, string arguments = null)
        {
            var runtimePath = PathUtility.GetRuntimePath();

            if (!Directory.Exists(Path.GetDirectoryName(runtimePath)))
                throw new DirectoryNotFoundException();

            return !string.IsNullOrEmpty(arguments)
                ? Process.Start(runtimePath, $"-jar {value} {arguments}")
                : Process.Start(runtimePath, $"-jar {value}");
        }
    }
}
