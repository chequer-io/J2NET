using System.Diagnostics;
using System.IO;
using J2NET.Exceptions;
using J2NET.Utilities;

namespace J2NET
{
    public static class JavaRuntime
    {
        public static Process ExecuteJar(string value, string arguments = null)
        {
            return Execute($"-jar {value}", arguments);
        }

        public static Process ExecuteClass(string value, string arguments = null)
        {
            return Execute($"-cp {value}", arguments);
        }

        public static ProcessStartInfo Create(string value, string arguments = null)
        {
            var runtimePath = PathUtility.GetRuntimePath();

            if (!Directory.Exists(Path.GetDirectoryName(runtimePath)))
                throw new RuntimeNotFoundException();

            var processArguments = value;

            if (!string.IsNullOrEmpty(arguments))
                processArguments += $" {arguments}";

            return new ProcessStartInfo(runtimePath, processArguments);
        }

        public static Process Execute(string value, string arguments = null)
        {
            var info = Create(value, arguments);
            return Process.Start(info);
        }
    }
}
