using System;
using System.Collections;
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
        public IDictionary errors;
        public VilIOExceptions() : base()
        {
            errors = new Dictionary<string, List<string>>();
        }
        public VilIOExceptions(string message) : base(message)
        {
            errors = new Dictionary<string, List<string>>();
        }
        public VilIOExceptions(string message, Exception inner) : base(message, inner)
        {
            errors = new Dictionary<string, List<string>>();
        }
        public VilIOExceptions(string message, List<string> listmsgs) : base(message)
        {
            errors = new Dictionary<string, List<string>>
            {
                { "validateErr", listmsgs }
            };
        }

        public override IDictionary Data => this.errors;
    }

    public class VilNotFoundExceptions : VilExceptions
    {
        public VilNotFoundExceptions() : base() {}
        public VilNotFoundExceptions(string message) : base(message) {}
        public VilNotFoundExceptions(string message, Exception inner) : base(message, inner) {}
    }
}
