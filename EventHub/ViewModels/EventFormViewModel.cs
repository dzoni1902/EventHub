using EventHub.Models;
using System.Collections.Generic;

namespace EventHub.ViewModels
{
    public class EventFormViewModel
    {
        public string Venue { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int Genre { get; set; }  //type is int - because of dropdown list (GenreId in DB)
        public IEnumerable<Genre> Genres { get; set; }
    }
}