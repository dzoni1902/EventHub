using EventHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using EventHub.Core;

namespace EventHub.Controllers.WebAPI
{
    [Authorize]
    public class EventsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
