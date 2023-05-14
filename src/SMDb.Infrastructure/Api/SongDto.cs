using AutoMapper;
using Newtonsoft.Json;
using SMDb.Domain;
using SMDb.Domain.Songs;

namespace SMDb.Infrastructure.Api;

public record SongDto(
    [JsonProperty("name")] string Name,
    [JsonProperty("artistName")] string ArtistName,
    [JsonProperty("genre")] string Genre)
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<SongDto, Song>()
                .ForCtorParam(nameof(Song.Name), opt => opt.MapFrom(src => src.Name))
                .ForCtorParam(nameof(Song.Artist), opt => opt.MapFrom(src => new Artist(src.ArtistName)))
                .ForCtorParam(nameof(Song.Genre), opt => opt.MapFrom(src => new Genre(src.Genre)));
        }
    }
}
