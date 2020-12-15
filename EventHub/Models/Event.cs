using System;
using System.ComponentModel.DataAnnotations;

namespace EventHub.Models
{
    public class Event
    {
        public int Id { get; set; }

        public bool IsCanceled { get; set; }

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
    }
}