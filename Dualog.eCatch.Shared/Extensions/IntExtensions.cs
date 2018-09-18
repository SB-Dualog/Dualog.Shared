namespace Dualog.eCatch.Shared.Extensions
{
    public static class IntExtensions
    {
        public static string WithThousandSeparator(this int val)
        {
            return val.ToString("N", Constants.IntNumberFormat);
        }

        public static string WithThousandSeparator(this int? val)
        {
            if (!val.HasValue) return "";
            return val.Value.ToString("N", Constants.IntNumberFormat);
        }
    }
}
