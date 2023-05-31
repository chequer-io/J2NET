using System;

namespace J2NET.Exceptions
{
    public class RuntimeNotFoundException : Exception
    {
        public RuntimeNotFoundException() : base("J2NET.Runtime could not be found. Please check if the runtime is installed for your platform.")
        {
        }
    }
}
