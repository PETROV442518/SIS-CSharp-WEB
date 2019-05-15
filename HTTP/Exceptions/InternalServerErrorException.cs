using System;
using System.Collections.Generic;
using System.Text;

namespace HTTP.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        private const string InternalServerErrorExceptionMessage = "The server has encountered an error";

        public InternalServerErrorException() : this(InternalServerErrorExceptionMessage)
        {

        }

        public InternalServerErrorException(string message) : base(message)
        {
        }
    }
}
