using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Exceptions
{
    public class ResponseAlreadyStartedException : Exception
    {
        public ResponseAlreadyStartedException(string message = "The requested resource was not found.")
            : base(message) { }
    }
}
