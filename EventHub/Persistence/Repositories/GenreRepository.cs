using EventHub.Core;
using EventHub.Core.Models;
using EventHub.Core.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace EventHub.Persistence.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly IApplicationDbContext _context;

        public GenreRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Genre> GetGenres()
        {
            return _context.Genres.ToList();
        }
    }
}