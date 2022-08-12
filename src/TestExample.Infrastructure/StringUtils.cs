namespace TestExample.Infrastructure
{
    public static class StringUtils
    {
        public static bool IsBlank(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string NormalizeText(this string value)
        {
            return value.IsBlank()
                ? string.Empty
                : value.Replace(" ", "").ToLower();
        }
    }
}