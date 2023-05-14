using SMDb.Domain;
using SMDb.Domain.Songs;

namespace SMDb.Application.Interfaces
{
    public interface ISMDbApi
    {
        public Task<List<Song>?> GetSongsAsync(List<Genre>? genres, CancellationToken cancellationToken);
    }
}
