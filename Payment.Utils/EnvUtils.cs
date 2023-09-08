namespace Payment.Utils
{
    public static class EnvUtils
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