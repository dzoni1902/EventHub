using System.Collections.Generic;
using EventHub.Core.Models;

namespace EventHub.Core.RepositoryInterfaces
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetNewNotifications(string userId);
        IEnumerable<Notification> GetRecentNotifications(string userId);
        IEnumerable<UserNotification> GetUserNotifications(string userId);
    }
}