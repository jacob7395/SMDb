namespace SMDb.Infrastructure.Config;

public class AppSettingsConfig
{
    public AppSettingsConfig()
    {
        Values = new Dictionary<string, string>();
    }
    public AppSettingsConfig(IReadOnlyDictionary<string, string> values)
    {
        Values = values;
    }

    public IReadOnlyDictionary<string, string> Values { get; }
}
