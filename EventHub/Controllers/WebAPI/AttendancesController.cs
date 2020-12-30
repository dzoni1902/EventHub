using EventHub.Dtos;
using EventHub.Models;
using EventHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace EventHub.Controllers.WebAPI
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            //userId from server, not from params, because of security reasons

            var userId = User.Identity.GetUserId();

            var exists = _unitOfWork.Attendances.GetAttendance(userId, dto.EventId) != null;
            if (exists)
            {
                return BadRequest("The attendance already exists.");
            }

            var attendence = new Attendance
            {
                AttendeeId = userId,
                EventId = dto.EventId
            };

            _unitOfWork.Attendances.Add(attendence);
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteAttendance(int id)
        {
            var userId = User.Identity.GetUserId();
            var attendance = _unitOfWork.Attendances.GetAttendance(userId, id);

            if (attendance == null)
            {
                return NotFound();
            }

            _unitOfWork.Attendances.Remove(attendance);
            _unitOfWork.Complete();

            return Ok(id);
        }
    }
}
