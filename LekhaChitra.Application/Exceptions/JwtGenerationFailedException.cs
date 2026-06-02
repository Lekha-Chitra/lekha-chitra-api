using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Exceptions
{
    public class JwtGenerationFailedException : Exception
    {
        public JwtGenerationFailedException(string message = "Failed to generate JWT token.")
            : base(message) { }
    }
}
