using System;
using EventHub.Models;

namespace EventHub.Dtos
{
    //easiest way to create DTO is to copy properties from original class
    //and delete those we don't want to show to the client
    public class NotificationDto
    {
        //Id removed, no need to return it 
        public DateTime DateTime { get; set; }
        public NotificationType Type { get; set; }

        public DateTime? OriginalDateTime { get; set; }
        public string OriginalVenue { get; set; }

        public EventDto Event { get; set; }
    }
}