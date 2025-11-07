using System;

namespace Gimnasio.Core.Exceptions
{
    public class BussinesException : Exception
    {
        public BussinesException()
        {
            
        }

        public BussinesException(string message)
            : base(message)
        {

        }

        public BussinesException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}