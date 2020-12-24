using AutoMapper;
using EventHub.Dtos;
using EventHub.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace EventHub.Controllers.WebAPI
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }

        //APIs should receive and return DTOs, not domain objects because
        //they are internal of the app, could be changed, and expose to much...

        public NotificationWrapperDto GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();

            var newNotifications = _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .Select(un => un.Notification)
                .Include(n => n.Event.Artist)
                .ToList();

            int newNotificationsCount = newNotifications.Count;

            if (newNotificationsCount == 0)
            {
                var recentNotifications = _context.UserNotifications
                    .Where(un => un.UserId == userId)
                    .Select(un => un.Notification)
                    .Include(n => n.Event.Artist)
                    .OrderByDescending(n => n.DateTime)
                    .Take(4)
                    .ToList();

                return new NotificationWrapperDto
                {
                    NotificationCounter = newNotificationsCount,
                    Notifications = recentNotifications.Select(Mapper.Map<Notification, NotificationDto>)
                };
            }

            return new NotificationWrapperDto
            {
                NotificationCounter = newNotificationsCount,
                Notifications = newNotifications.Select(Mapper.Map<Notification, NotificationDto>)
            };
        }

        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var userId = User.Identity.GetUserId();

            var notifications = _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .ToList();

            notifications.ForEach(un => un.Read());

            _context.SaveChanges();

            return Ok();
        }
    }
}
