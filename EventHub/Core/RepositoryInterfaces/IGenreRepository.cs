using System.Collections.Generic;
using EventHub.Core.Models;

namespace EventHub.Core.RepositoryInterfaces
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetGenres();
    }
}