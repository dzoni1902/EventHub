using EventHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using EventHub.Core;
using EventHub.Core.Dtos;
using EventHub.Core.Models;

namespace EventHub.Controllers.WebAPI
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendancesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
