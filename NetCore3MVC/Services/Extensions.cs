namespace NetCore3MVC.Services
{
    public static class Extensions
    {
        public static bool ContainsCaseInsensitive(this string source, string substring)
        {
            return source?.IndexOf(substring, System.StringComparison.OrdinalIgnoreCase) > -1;
        }
    }
}
