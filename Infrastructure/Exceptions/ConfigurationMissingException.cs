using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections;
using System.Runtime.Serialization;

namespace Infrastructure.Exceptions
{
    [Serializable]
    public class ConfigurationMissingException: ConfigurationErrorsException
    {
        public ConfigurationMissingException()
        {
            
        }

        public ConfigurationMissingException(string? message):base(message) 
        {
            
        }

        public ConfigurationMissingException(string? message, Exception innerException) : base(message, innerException)
        {

        }

        public ConfigurationMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }


    }

}
