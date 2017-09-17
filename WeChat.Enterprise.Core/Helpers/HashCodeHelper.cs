using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    class HashCodeHelper
    {
        public static int HashCode(params object[] args)
        {
            int hash = 0;
            foreach (var item in args)
            {
                hash = hash * 31 + item.GetHashCode();
            }
            return hash;
        }
    }
}
