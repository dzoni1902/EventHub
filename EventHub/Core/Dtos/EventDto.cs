using System;

namespace EventHub.Core.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }

        public bool IsCanceled { get; set; }

        public UserDto Artist { get; set; }

        //foreign key props from model are not necessary in DTO, ther are for optimisation
        public DateTime DateTime { get; set; }

        public string Venue { get; set; }

        public GenreDto Genre { get; set; }

        //Attendances prop also not needed
    }
}