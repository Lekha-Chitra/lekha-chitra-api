using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "The requested resource was not found.")
            : base(message) { }
    }
}
