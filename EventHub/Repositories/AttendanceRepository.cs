using EventHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventHub.Repositories
{
    public class AttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Attendance> GetFutureAttendances(string userId)
        {
            return _context.Attendances
                .Where(a => a.AttendeeId == userId && a.Event.DateTime > DateTime.Now).ToList();
        }

        public Attendance GetAttendance(string userId, int eventObjectId)
        {
            return _context.Attendances
                .SingleOrDefault(a => a.AttendeeId == userId && a.EventId == eventObjectId);
        }
    }
}