using System.Collections.Generic;
using EventHub.Core.Models;

namespace EventHub.Core.RepositoryInterfaces
{
    //any of the repo Ifaces has no dependencies on the dbContext or entity FW
    public interface IEventRepository
    {
        IEnumerable<Event> GetEventsUserAttending(string userId);
        Event GetEventWithAtendees(int eventId);
        Event GetEvent(int eventId);
        IEnumerable<Event> GetUpcomingEventsByArtist(string userId);
        void Add(Event eventObject);
        IEnumerable<Event> GetUpcomingEvents();
    }
}