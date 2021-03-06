using EventHub.Core;
using EventHub.Core.Models;
using EventHub.Core.RepositoryInterfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EventHub.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IApplicationDbContext _context;

        public NotificationRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Notification> GetNewNotifications(string userId)
        {
            return _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .Select(un => un.Notification)
                .Include(n => n.Event.Artist)
                .ToList();
        }

        public IEnumerable<Notification> GetRecentNotifications(string userId)
        {
            return _context.UserNotifications
                .Where(un => un.UserId == userId)
                .Select(un => un.Notification)
                .Include(n => n.Event.Artist)
                .OrderByDescending(n => n.DateTime)
                .Take(4)
                .ToList();
        }

        public IEnumerable<UserNotification> GetUserNotifications(string userId)
        {
            return _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .ToList();
        }
    }
}