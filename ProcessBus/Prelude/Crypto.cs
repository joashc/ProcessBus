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
        public static readonly Func<string, byte[]> Sha256Hash = s =>
        {
            using (var sha256 = new SHA256CryptoServiceProvider())
            {
                return compose(StringBytes, sha256.ComputeHash)(s);
            }
        };
    }
}
