using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoIFClient
{
    internal class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() : base()
        {
            
        }

        public InvalidPasswordException(string message) : base(message)
        {

        }

        public InvalidPasswordException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
