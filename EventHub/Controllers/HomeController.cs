using EventHub.Models;
using EventHub.Persistence;
using EventHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;


namespace EventHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public HomeController()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
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