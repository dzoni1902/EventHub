using EventHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using EventHub.Core;
using EventHub.Core.ViewModels;


namespace EventHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(string query = null)
        {
            var upcomingEvents = _unitOfWork.Events.GetUpcomingEvents();

            //if there is a query, we need to apply a filter
            if (!string.IsNullOrWhiteSpace(query))
            {
                upcomingEvents = upcomingEvents
                    .Where(e => e.Artist.Name.ToLowerInvariant().Contains(query.ToLowerInvariant())
                                || e.Genre.Name.ToLowerInvariant().Contains(query.ToLowerInvariant())
                                || e.Venue.ToLowerInvariant().Contains(query.ToLowerInvariant()))
                    .ToList();
            }

            var userId = User.Identity.GetUserId();
            var attendances = _unitOfWork.Attendances.GetFutureAttendances(userId).ToLookup(a => a.EventId);

            var viewModel = new EventsViewModel
            {
                UpcomingEvents = upcomingEvents,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming Events",
                SearchTerm = query,      //to autopopulate search box
                Attendances = attendances
            };

            return View("Events", viewModel);
        }
    }
}