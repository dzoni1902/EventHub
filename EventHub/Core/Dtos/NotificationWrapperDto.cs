using System.Collections.Generic;

namespace EventHub.Core.Dtos
{
    public class NotificationWrapperDto
    {
        public int NotificationCounter { get; set; }
        public IEnumerable<NotificationDto> Notifications { get; set; } = new List<NotificationDto>();
    }
}