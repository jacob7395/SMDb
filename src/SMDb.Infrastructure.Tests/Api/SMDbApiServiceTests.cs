using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using SMDb.Application.Interfaces;
using SMDb.Domain.Songs;
using SMDb.Infrastructure.Api;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SMDb.Infrastructure.Tests.Api;

[TestFixture]
public class SMDbApiServiceTests
{
    [SetUp]
    public void Setup()
    {
        Substitute.For<HttpMessageHandler>();
        _clientFactory = Substitute.For<IHttpClientFactory>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<SongDto.Mapper>();
        });

        config.AssertConfigurationIsValid();

        _mapper = config.CreateMapper();

        _configuration = Substitute.For<IConfigurationAccessor>();

        string? token = null;

        Assert.NotNull(token, "Set token value.");
        Assert.Fail("Need to reconfigure to setup HTTP client like in startup");

        _configuration.GetVariable("SMDbBearer").Returns(token ?? "");

        _logger = Substitute.For<ILogger>();

        _service = new(_mapper, new SMDbApiClient(new HttpClient(), _configuration), _configuration, _logger);

    }

    [TearDown]
    public void Cleanup()
    {
        _service = null;
        _clientFactory = null;
        _mapper = null;
        _configuration = null;
    }

    private SMDbApiService _service;
    private IHttpClientFactory _clientFactory;
    private IMapper _mapper;
    private IConfigurationAccessor _configuration;
    private ILogger _logger;

    [Test]
    public async Task GetSongs_ReturnsSongs()
    {
        var httpClient = new HttpClient { BaseAddress = new("https://smdb.azurewebsites.net/api") };

        _clientFactory.CreateClient().Returns(httpClient);

        var result = await _service.GetSongsAsync(null, new CancellationToken());

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.All(song => !string.IsNullOrEmpty(song.Name)));
        Assert.That(result.All(song => song.Artist != null && !string.IsNullOrEmpty(song.Artist.Name)));
        Assert.That(result.All(song => song.Genre != null && !string.IsNullOrEmpty(song.Genre.Name)));
    }

    [Test]
    public async Task GetSongs_WithGenres_ReturnsOk()
    {
        // Arrange
        var httpClient = new HttpClient { BaseAddress = new("https://smdb.azurewebsites.net/api") };
        _clientFactory.CreateClient().Returns(httpClient);

        var genres = new List<Genre> { new("Rock"), new("Pop") };

        // Act
        var result = await _service.GetSongsAsync(genres, new CancellationToken());

        Assert.That(result, Is.Not.Empty);
        Assert.That(result.All(song => !string.IsNullOrEmpty(song.Name)));
        Assert.That(result.All(song => song.Artist != null && !string.IsNullOrEmpty(song.Artist.Name)));
        Assert.That(result.All(song => song.Genre != null && !string.IsNullOrEmpty(song.Genre.Name)));
    }
}
