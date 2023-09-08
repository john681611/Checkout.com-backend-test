namespace Payment.Utils
{
    public static class Utils
    {
        public static string GetRequiredEnvironmentVariable(string key)
        {
            var value = Environment.GetEnvironmentVariable(key);
            if (value is null)
                throw new ArgumentNullException($"Environment Variable: {key}");
            return value;
        }
    }
}