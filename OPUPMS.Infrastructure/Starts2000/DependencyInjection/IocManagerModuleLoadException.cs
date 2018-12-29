using System;
using System.Runtime.Serialization;

namespace Starts2000.DependencyInjection
{
    public class IocManagerModuleLoadException : Exception
    {
        public IocManagerModuleLoadException()
        {

        }

        public IocManagerModuleLoadException(string message) : base(message)
        {

        }

        public IocManagerModuleLoadException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected IocManagerModuleLoadException(
            SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
