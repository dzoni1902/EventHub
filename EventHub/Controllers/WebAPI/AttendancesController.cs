using EventHub.Dtos;
using EventHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace EventHub.Controllers.WebAPI
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            //userId from server, not from params, because of security reasons

            var userId = User.Identity.GetUserId();
            var exists = _context.Attendances.Any(a => a.AttendeeId == userId && a.EventId == dto.EventId);

            if (exists)
            {
                return BadRequest("The attendance already exists.");
            }

            var attendence = new Attendance
            {
                AttendeeId = userId,
                EventId = dto.EventId
            };
            _context.Attendances.Add(attendence);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteAttendance(int id)
        {
            var userId = User.Identity.GetUserId();
            var attendance = _context.Attendances
                .SingleOrDefault(a => a.EventId == id && a.AttendeeId == userId);

            if (attendance == null)
            {
                return NotFound();
            }

            _context.Attendances.Remove(attendance);
            _context.SaveChanges();
            return Ok(id);
        }
    }
}
