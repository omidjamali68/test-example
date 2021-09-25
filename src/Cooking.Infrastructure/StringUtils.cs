namespace Cooking.Infrastructure
{
    public static class StringUtils
    {
        public static bool IsBlank(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string ValueOrEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }
    }
}