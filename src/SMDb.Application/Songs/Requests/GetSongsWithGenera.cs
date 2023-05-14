using MediatR;
using SMDb.Application.Interfaces;
using SMDb.Domain;
using SMDb.Domain.Songs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMDb.Application.Songs.Requests
{
    public record GetSongsQuery(List<Domain.Songs.Genre>? Genres = null) : IRequest<List<Song>>
    {
        public class Handler : IRequestHandler<GetSongsQuery, List<Song>>
        {
            private readonly ISMDbApi _api;

            public Handler(ISMDbApi api)
            {
                _api = api;
            }

            public async Task<List<Song>> Handle(GetSongsQuery request, CancellationToken cancellationToken)
            {
                return await _api.GetSongsAsync(request.Genres, cancellationToken) ?? new List<Song>();
            }
        }
    }
    
}
