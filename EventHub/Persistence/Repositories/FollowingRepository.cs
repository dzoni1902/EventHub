﻿using EventHub.Core;
using EventHub.Core.Models;
using EventHub.Core.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace EventHub.Persistence.Repositories
{
    public class FollowingRepository : IFollowingRepository
    {
        private readonly IApplicationDbContext _context;

        public FollowingRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public Following GetFollowing(string userId, string artistId)
        {
            return _context.Followings
                .SingleOrDefault(f => f.FollowerId == userId && f.FolloweeId == artistId);
        }

        public IEnumerable<ApplicationUser> GetArtistsIFollow(string userId)
        {
            return _context.Followings
                .Where(f => f.FollowerId == userId)
                .Select(f => f.Followee)
                .ToList();
        }

        public void Add(Following following)
        {
            _context.Followings.Add(following);
        }

        public void Remove(Following following)
        {
            _context.Followings.Remove(following);
        }
    }
}