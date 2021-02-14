using System;
using System.Collections.Generic;
using System.Linq;
using EventHub.Core.Models;
using EventHub.Core.RepositoryInterfaces;

namespace EventHub.Persistence.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
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

        public void Add(Attendance attendence)
        {
            _context.Attendances.Add(attendence);
        }

        public void Remove(Attendance attendance)
        {
            _context.Attendances.Remove(attendance);
        }
    }
}