using System;
using static LanguageExt.Prelude;

namespace ProcessBus
{
    public static partial class Prelude
    {
        public static string ProcessSuffix()
        {
            return compose(Sha256Hash, ToLowerHexString, curry(TakeString)(10))(Guid.NewGuid().ToString());
        }
         
    }
}