using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViL.Common.Commons
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class VilUnchanged : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class HasNoKey : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AsKey : Attribute { }
}
