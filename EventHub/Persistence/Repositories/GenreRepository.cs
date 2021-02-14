using System.Collections.Generic;
using System.Linq;
using EventHub.Core.Models;
using EventHub.Core.RepositoryInterfaces;

namespace EventHub.Persistence.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Genre> GetGenres()
        {
            return _context.Genres.ToList();
        }
    }
}