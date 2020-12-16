using System;
using System.ComponentModel.DataAnnotations;

namespace EventHub.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType Type { get; private set; }

        //null-able props for update scenario
        public DateTime? OriginalDateTime { get; set; }
        public string OriginalVenue { get; set; }

        //each notification is for only 1 event
        [Required]
        public Event Event { get; private set; }

        //for entity FW
        protected Notification()
        {
        }

        public Notification(Event eventObj, NotificationType type)
        {
            Event = eventObj ?? throw new ArgumentNullException(nameof(eventObj));
            Type = type;
            DateTime = DateTime.Now;
        }
    }
}