

namespace Client.Extensions
{
    public static class DateTimeExtensions
    {
        private const string Format = "HH:mm:ss ffffff";

        public static string ToExactString(this DateTime dateTime)
        {
            return dateTime.ToString(Format);
        }

        public static DateTime FromExactString(this string str)
        {
            return DateTime.ParseExact(str, Format, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
