using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SMDb.Infrastructure.Config;

namespace SMDb;

public static class WebAppBuilderExtension
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        builder.Host.UseSerilog(logger);
    }
    public static void BindAppSettingConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ConfigurationAccessor>();

        Dictionary<string, string> appSettingValues = builder.Configuration.GetChildren()
            .Where(m => m.Value is { })
            .ToDictionary(x => x.Key, x => x.Value)!;

        appSettingValues["GitHash"] = builder.Configuration.GetValue<string>("GitHash") ?? "GitHash not set";

        builder.Services.AddTransient(config => new AppSettingsConfig(appSettingValues));
    }
}
