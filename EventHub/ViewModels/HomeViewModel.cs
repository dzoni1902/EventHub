using System.Collections.Generic;
using EventHub.Models;

namespace EventHub.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Event> UpcomingEvents { get; set; }
        public bool ShowActions { get; set; }
    }
}