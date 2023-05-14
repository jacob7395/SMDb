using MediatR;
using SMDb.Domain.Songs;

namespace SMDb.Application.Genres.Requests
{
    public class GetValidGenresRequest : IRequest<List<Genre>>
    {
        public class Handler : IRequestHandler<GetValidGenresRequest, List<Genre>>
        {
            public Task<List<Genre>> Handle(GetValidGenresRequest request, CancellationToken cancellationToken) =>
                Task.FromResult(new List<Genre>()
                {
                    new("Indie"),
                    new("Rock"),
                    new("Pop"),
                });
        }
    }
}
