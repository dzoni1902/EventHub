using EventHub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EventHub.Repositories
{
    public class EventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
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
                .Where(e =>
                    e.ArtistId == userId &&
                    e.DateTime > DateTime.Now &&
                    !e.IsCanceled)
                .Include(e => e.Genre)
                .ToList();
        }
    }
}