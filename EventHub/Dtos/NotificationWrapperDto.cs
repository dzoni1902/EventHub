using System.Collections.Generic;

namespace EventHub.Dtos
{
    public class NotificationWrapperDto
    {
        public int NotificationCounter { get; set; }
        public IEnumerable<NotificationDto> Notifications { get; set; } = new List<NotificationDto>();
    }
}