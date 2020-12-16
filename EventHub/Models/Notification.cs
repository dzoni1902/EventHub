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
        public DateTime? OriginalDateTime { get; private set; }
        public string OriginalVenue { get; private set; }

        //each notification is for only 1 event
        [Required]
        public Event Event { get; private set; }

        //for entity FW
        protected Notification()
        {
        }

        private Notification(Event eventObj, NotificationType type)
        {
            Event = eventObj ?? throw new ArgumentNullException(nameof(eventObj));
            Type = type;
            DateTime = DateTime.Now;
        }

        //Factory methods to be used instead of constructor
        public static Notification EventCreated(Event eventObj)
        {
            return new Notification(eventObj, NotificationType.EventCreated);
        }

        public static Notification EventCanceled(Event eventObj)
        {
            return new Notification(eventObj, NotificationType.EventCanceled);
        }

        //this way I make sure always to have notification object in valid state
        //if we would have more params in this factory method, we could use newEvent and oldEvent as params.
        public static Notification EventUpdated(Event newEvent, DateTime origininalDateTime, string originalVenue)
        {
            var notification = new Notification(newEvent, NotificationType.EventUpdated);
            notification.OriginalDateTime = origininalDateTime;
            notification.OriginalVenue = originalVenue;

            return notification;
        }
    }
}