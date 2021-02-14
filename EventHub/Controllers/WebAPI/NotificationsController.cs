using AutoMapper;
using EventHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using EventHub.Core;
using EventHub.Core.Dtos;
using EventHub.Core.Models;

namespace EventHub.Controllers.WebAPI
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //APIs should receive and return DTOs, not domain objects because
        //they are internal of the app, could be changed, and expose to much...

        public NotificationWrapperDto GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();

            var newNotifications = _unitOfWork.Notifications.GetNewNotifications(userId).ToList();

            var newNotificationsCount = newNotifications.Count;

            if (newNotificationsCount == 0)
            {
                var recentNotifications = _unitOfWork.Notifications.GetRecentNotifications(userId);

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

            var notifications = _unitOfWork.Notifications.GetUserNotifications(userId).ToList();

            notifications.ForEach(un => un.Read());

            _unitOfWork.Complete();

            return Ok();
        }
    }
}
