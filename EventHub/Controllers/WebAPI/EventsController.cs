using EventHub.Core;
using Microsoft.AspNet.Identity;
using System.Web.Http;

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

            if (eventObject == null || eventObject.IsCanceled)
            {
                return NotFound();
            }

            if (eventObject.ArtistId != User.Identity.GetUserId())
            {
                return Unauthorized();
            }

            eventObject.CancelEvent();

            _unitOfWork.Complete();

            return Ok();
        }
    }
}
