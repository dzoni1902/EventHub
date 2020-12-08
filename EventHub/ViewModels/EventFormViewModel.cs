using EventHub.Models;
using System;
using System.Collections.Generic;

namespace EventHub.ViewModels
{
    public class EventFormViewModel
    {
        //view model is used for UI, to prevent adding new stuff and polute our domain classes
        public string Venue { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public byte Genre { get; set; }  //type is int - because of dropdown list (GenreId in DB)
        public IEnumerable<Genre> Genres { get; set; }

        public DateTime DateTime => DateTime.Parse(string.Format($"{Date}-{Time}"));
    }
}