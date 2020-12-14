using EventHub.Models;
using EventHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
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

        //Edit & Create actions take us to page where we CREATE new or EDIT existing event
        //these actions return VIEW's, GET actions
        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var eventObject = _context.Events.Single(e => e.Id == id && e.ArtistId == userId);

            //we need to initialize viewModel in order form to be populated in Edit page
            var viewModel = new EventFormViewModel
            {
                Id = eventObject.Id,
                Heading = "Edit an Event",
                Genres = _context.Genres.ToList(),
                Date = eventObject.DateTime.ToString("d MMM yyyy"),
                Time = eventObject.DateTime.ToString("HH:mm"),
                Genre = eventObject.GenreId,
                Venue = eventObject.Venue
            };

            return View("EventForm", viewModel);
        }

        //Action for clicking on Add an Event link in navbar
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new EventFormViewModel
            {
                Genres = _context.Genres.ToList(),
                Heading = "Add an Event",
            };

            return View("EventForm", viewModel);
        }

        //CREATE and UPDATE actions are POST actions and they SAVE form data to the DB
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
                return View("EventForm", viewModel);
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

            return RedirectToAction("Mine", "Events");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(EventFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                //we need to initialize and populate Genres prop since viewModel is 
                //new model initialized with values from the httpRequest
                viewModel.Genres = _context.Genres.ToList();
                return View("EventForm", viewModel);
            }

            var userId = User.Identity.GetUserId();
            var eventObject = _context.Events.Single(e => e.Id == viewModel.Id && e.ArtistId == userId);

            eventObject.Venue = viewModel.Venue;
            eventObject.DateTime = viewModel.GetDateTime();
            eventObject.GenreId = viewModel.Genre;

            _context.SaveChanges();

            return RedirectToAction("Mine", "Events");
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

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();

            var events = _context.Events
                .Where(e => e.ArtistId == userId && e.DateTime > DateTime.Now)
                .Include(e => e.Genre)
                .ToList();

            return View(events);
        }
    }
}