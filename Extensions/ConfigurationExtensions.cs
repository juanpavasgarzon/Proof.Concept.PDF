namespace File.Api.Extensions;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Gets the configuration value for the specified key.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string GetRequired(this IConfiguration config, string key)
    {
        var value = config[key];

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Missing required configuration key: '{key}'");
        }

        return value;
    }
}