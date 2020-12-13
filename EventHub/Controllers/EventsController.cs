using EventHub.Models;
using EventHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace EventHub.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController()
        {
            _context = new ApplicationDbContext();
        }


        //Action for clicking on Add an Event link in navbar
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new EventFormViewModel
            {
                Genres = _context.Genres.ToList()
            };

            return View(viewModel);
        }

        //convert viewModel to Event object, save to context (DB)
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                //we need to initialize and populate Genres prop since viewModel is 
                //new model initialized with values from the httpRequest
                viewModel.Genres = _context.Genres.ToList();
                return View("Create", viewModel);
            }

            var eventObject = new Event
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _context.Events.Add(eventObject);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var events = _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Event)
                .Include(e => e.Artist)
                .Include(e => e.Genre)
                .ToList();

            var viewModel = new EventsViewModel
            {
                UpcomingEvents = events,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Events I'm attending"
            };

            return View("Events", viewModel);
        }
    }
}