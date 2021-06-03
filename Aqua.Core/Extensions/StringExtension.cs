namespace Aqua.Core.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string text) 
            => string.IsNullOrEmpty(text);
    }
}