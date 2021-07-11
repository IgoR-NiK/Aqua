namespace Aqua.Core.Utils
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string text) 
            => string.IsNullOrEmpty(text);

        public static bool IsNullOrWhiteSpace(this string text)
            => string.IsNullOrWhiteSpace(text);
    }
}