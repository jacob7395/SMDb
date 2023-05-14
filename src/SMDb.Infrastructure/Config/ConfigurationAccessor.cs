using SMDb.Application.Interfaces;

namespace SMDb.Infrastructure.Config;

public class ConfigurationAccessor : IConfigurationAccessor
{
    private readonly AppSettingsConfig _appSettings;
    private readonly ILogger _logger;

    public ConfigurationAccessor(AppSettingsConfig appSettings, ILogger logger)
    {
        _appSettings = appSettings;
        _logger = logger;
    }

    public string GetVariable(string name)
    {
        var value = TryGetVariable(name);

        if (value is null)
        {
            var e = new InvalidDataException($"Environment variable with name {name} was not set.");
            _logger.Error(e, $"Failed to read environment variable with name {name}.");
            throw e;
        }

        return value;
    }

    private string? TryGetVariable(string name) =>
        _appSettings.Values.ContainsKey(name)
            ? _appSettings.Values[name]
            : Environment.GetEnvironmentVariable(name);
}
