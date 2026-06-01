using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(
            string message = "The request was not valid. Please verify and try again."
        )
            : base(message) { }
    }
}
