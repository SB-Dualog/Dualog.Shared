using System;

namespace Dualog.eCatch.Shared.Enums
{
    public static class EnumHelper
    {
        public static T Parse<T>(string enumValue) where T : struct
        {
            return (T) Enum.Parse(typeof (T), enumValue);
        }
    }
}
