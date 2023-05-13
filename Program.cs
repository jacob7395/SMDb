using MudBlazor.Services;
using SMDb;
using SMDb.Application;
using SMDb.Application.Interfaces;
using SMDb.Infrastructure;
using SMDb.Infrastructure.Api;
using SMDb.Infrastructure.Config;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddMudServices();

builder.Services.AddSingleton<IConfigurationAccessor, ConfigurationAccessor>();
builder.Services.AddScoped<ISMDbApi, SMDbApiService>();
builder.Services.AddScoped<SMDbApiClient>();

builder.Services.AddHttpClient<SMDbApiClient>((sp, client) =>
{
    var configuration = sp.GetRequiredService<IConfigurationAccessor>();
    client.BaseAddress = new Uri("https://smdb.azurewebsites.net/api");
    var bearerToken = configuration.GetVariable("SMDbBearer");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
});

builder.Services.AddAutoMapper(SMDbInfrastructureEntryPoint.Assembly);
builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssembly(SMDbApplicationEntryPoint.Assembly));


builder.BindAppSettingConfig();
builder.ConfigureLogging();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
