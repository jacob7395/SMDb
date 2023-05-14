using SMDb.Domain.Songs;

namespace SMDb.Domain;

public record Song(string Name, Artist Artist, Genre Genre);
