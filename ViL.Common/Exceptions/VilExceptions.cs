using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViL.Common.Exceptions
{
    public class VilExceptions : Exception
    {
        public VilExceptions() : base() {}
        public VilExceptions(string message) : base(message) {}
        public VilExceptions(string mesage, Exception inner) : base(mesage, inner) {}

    }

    public class VilUnauthorizeExceptions : VilExceptions
    {
        public VilUnauthorizeExceptions() : base() {}
        public VilUnauthorizeExceptions(string message) : base(message) {}
        public VilUnauthorizeExceptions(string message, Exception inner) : base(message, inner) {}
    }   

    public class VilIOExceptions : VilExceptions
    {
        public VilIOExceptions() : base() {}
        public VilIOExceptions(string message) : base(message) {}
        public VilIOExceptions(string message, Exception inner) : base(message, inner) {}
    }
}
