using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace webtransformationadmin.Domain
{
    public class Utility
    {
        public static bool IsNullOrEmpty(string s)
        {
            bool b = false;
            if (s == null || s.Length == 0)
            {
                b = true;
            }
            return b;
        }
    }
}
