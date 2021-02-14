using System.Collections.Generic;
using EventHub.Core.Models;

namespace EventHub.Core.RepositoryInterfaces
{
    public interface IFollowingRepository
    {
        Following GetFollowing(string userId, string artistId);
        IEnumerable<ApplicationUser> GetArtistsIFollow(string userId);
        void Add(Following following);
        void Remove(Following following);
    }
}