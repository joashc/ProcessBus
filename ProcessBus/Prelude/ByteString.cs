using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ProcessBus
{
    public static partial class Prelude
    {
        public static readonly Func<string, byte[]> StringBytes = new UTF8Encoding().GetBytes;
        public static readonly Func<byte[], string> ToLowerHexString = bs => bs.Aggregate(new StringBuilder(32), (sb, b) => sb.Append(b.ToString("x2"))).ToString();

        public static readonly Func<int, string, string> TakeString = (n, s) =>
        {
            if (s == null || s.Length <= n) return s;
            return s.Substring(0, n);
        };
    }
}
