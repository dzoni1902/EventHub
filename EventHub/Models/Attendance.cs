using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventHub.Models
{
    public class Attendance
    {
        //here we'll need navigation properties
        public ApplicationUser Attendee { get; set; }
        public Event Event { get; set; }

        //foreign key props, for optimisation purpose
        [Key]
        [Column(Order = 1)]
        public int EventId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string AttendeeId { get; set; }

        //If we delete user, that causes cascade delete of events and attendencies
        //This has to be changed in fluent api because attendencies are trying to
        //be deleted on few paths (removing user or an event)
        //Events should be canceled, not deleted from the DB
    }
}