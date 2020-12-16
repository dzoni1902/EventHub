using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EventHub.Models
{
    public class Event
    {
        public int Id { get; set; }

        //after implementing cancel method, this shouldn't be able to modify outside the class
        public bool IsCanceled { get; private set; }

        public ApplicationUser Artist { get; set; }     //navigation prop

        [Required]
        public string ArtistId { get; set; }    //string - because in ApplicationUser, Id is a string (Guid)

        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        public Genre Genre { get; set; }    //navigation prop

        [Required]
        public byte GenreId { get; set; }    //this and ArtistId are foreign keys

        //private so i can't be overwritten with another collection
        public ICollection<Attendance> Attendances { get; private set; }

        public Event()
        {
            Attendances = new Collection<Attendance>();
        }

        public void CancelEvent()
        {
            IsCanceled = true;

            //create notification about cancelation
            var notification = Notification.EventCanceled(this);

            //for each attendee we need to create UserNotification object
            //by delegating to the domain (OOP) instead of having fat controlers
            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }


        public void Modify(string venue, DateTime dateTime, byte genre)
        {
            var notification = Notification.EventUpdated(this, this.DateTime, this.Venue);

            Venue = venue;
            DateTime = dateTime;
            GenreId = genre;

            //for each attendee we need to create UserNotification object
            //by delegating to the domain (OOP) we keep clean controlers
            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }
    }
}