using System.Collections.Generic;
using EventHub.Core.Models;

namespace EventHub.Core.RepositoryInterfaces
{
    public interface IAttendanceRepository
    {
        IEnumerable<Attendance> GetFutureAttendances(string userId);
        Attendance GetAttendance(string userId, int eventObjectId);
        void Add(Attendance attendence);
        void Remove(Attendance attendance);
    }
}