using EventHub.Core.Models;
using EventHub.Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EventHub.Persistence.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IApplicationDbContext _context;

        public EventRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Event> GetEventsUserAttending(string userId)
        {
            return _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Event)
                .Include(e => e.Artist)
                .Include(e => e.Genre)
                .ToList();
        }

        public Event GetEventWithAtendees(int eventId)
        {
            return _context.Events
                .Include(e => e.Attendances.Select(a => a.Attendee))
                .SingleOrDefault(e => e.Id == eventId);
        }

        public Event GetEvent(int eventId)
        {
            return _context.Events
                .Include(e => e.Genre)
                .Include(e => e.Artist)
                .SingleOrDefault(e => e.Id == eventId);
        }


        public IEnumerable<Event> GetUpcomingEventsByArtist(string userId)
        {
            return _context.Events
                .Where(e => e.ArtistId == userId &&
                    e.DateTime > DateTime.Now &&
                    !e.IsCanceled)
                .Include(e => e.Genre)
                .ToList();
        }

        public void Add(Event eventObject)
        {
            _context.Events.Add(eventObject);
        }

        public IEnumerable<Event> GetUpcomingEvents()
        {
            return _context.Events
                .Include(e => e.Artist)
                .Include(e => e.Genre)
                .Where(e => e.DateTime > DateTime.Now && !e.IsCanceled)
                .ToList();
        }
    }
}