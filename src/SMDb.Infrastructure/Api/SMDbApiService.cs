using AutoMapper;
using Newtonsoft.Json;
using SMDb.Application.Interfaces;
using SMDb.Domain;
using SMDb.Domain.Songs;
using System.Text.Json;
using System.Web;

namespace SMDb.Infrastructure.Api;

public class SMDbApiService : ISMDbApi
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfigurationAccessor _configuration;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly SMDbApiClient _client;

    public SMDbApiService(IMapper mapper, SMDbApiClient client, IConfigurationAccessor configuration,
        ILogger logger)
    {
        _mapper = mapper;
        _client = client;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<List<Song>?> GetSongsAsync(List<Genre>? genres, CancellationToken cancellationToken)
    {
        _logger.Debug($"Fetching song data for genres - {string.Join(',', genres ?? new List<Genre>())}");

        var request = CreateSongsRequest(genres);

        var response = await _client.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            _logger.Debug("Successfully got songs from SMDb API.");
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var songsDto = JsonConvert.DeserializeObject<List<SongDto>>(content);
            return _mapper.Map<List<Song>>(songsDto);
        }

        _logger.Error($"Failed to get songs from SMDb API. Status code: {response.StatusCode}.");
        return null;
    }

    private HttpRequestMessage CreateSongsRequest(List<Genre>? genres)
    {
        var uriBuilder = new UriBuilder("https://smdb.azurewebsites.net/api/songs");

        if (genres != null)
        {
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["genre"] = string.Join(",", genres.Select(g => g.Name));
            uriBuilder.Query = query.ToString();
        }

        var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.ToString());

        var bearerToken = _configuration.GetVariable("SMDbBearer");
        request.Headers.Authorization = new("Bearer", bearerToken);

        return request;
    }
}
