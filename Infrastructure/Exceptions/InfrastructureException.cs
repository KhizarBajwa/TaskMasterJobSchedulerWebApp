using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    internal class InfrastructureException : Exception
    {
        public InfrastructureException()
        {

        }

        public InfrastructureException(string? message, Exception? innerException) : base(message, innerException)
        {

        }

        public InfrastructureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
