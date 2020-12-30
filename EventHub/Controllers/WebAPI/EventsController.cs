using EventHub.Models;
using EventHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace EventHub.Controllers.WebAPI
{
    [Authorize]
    public class EventsController : ApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public EventsController()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var eventObject = _unitOfWork.Events.GetEventWithAtendees(id);

            if (eventObject == null)
            {
                return NotFound();
            }

            if (eventObject.ArtistId != User.Identity.GetUserId())
            {
                return BadRequest("Unauthorized access!");
            }

            if (eventObject.IsCanceled)
            {
                return NotFound();
            }

            eventObject.CancelEvent();

            _unitOfWork.Complete();

            return Ok();
        }
    }
}
