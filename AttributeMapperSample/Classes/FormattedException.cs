using System;

namespace AttributeMapperSample.Classes
{
    public class FormattedException : Exception
    {

        public FormattedException(string message, params object[] args) 
            : base(string.Format(message,args))  {}

        public FormattedException(string message, Exception innerException, params object[] args)
            : base(string.Format(message,args),innerException) {}

    }
}
