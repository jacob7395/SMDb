using SMDb.Application.Interfaces;
using System.Net.Http.Headers;

namespace SMDb.Infrastructure.Api;

public class SMDbApiClient : HttpClient
{
    private readonly HttpClient _client;

    public SMDbApiClient(HttpClient client, IConfigurationAccessor configuration)
    {
        client.BaseAddress = new Uri("https://smdb.azurewebsites.net/api");
        var bearerToken = configuration.GetVariable("SMDbBearer");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        _client = client;
    }
    //public SMDbApiClient(HttpClient client, IConfigurationAccessor configuration)
    //{
    //    client.BaseAddress = new Uri("https://smdb.azurewebsites.net/api");
    //    var bearerToken = configuration.GetVariable("SMDbBearer");
    //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
    //    BaseAddress = client.BaseAddress;
    //    DefaultRequestHeaders.Clear();
    //    foreach (var header in client.DefaultRequestHeaders)
    //    {
    //        DefaultRequestHeaders.Add(header.Key, header.Value.ToArray());
    //    }
    //}
}
