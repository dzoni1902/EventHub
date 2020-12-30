using EventHub.Models;
using EventHub.Repositories;
using EventHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace EventHub.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttendanceRepository _attendanceRepository;
        private readonly EventRepository _eventRepository;
        private readonly GenreRepository _genreRepository;
        private readonly FollowingRepository _followingRepository;

        public EventsController()
        {
            _context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(_context);
            _eventRepository = new EventRepository(_context);
            _genreRepository = new GenreRepository(_context);
            _followingRepository = new FollowingRepository(_context);
        }

        //Edit & Create actions take us to page where we CREATE new or EDIT existing event
        //these actions return VIEW's, GET actions

        public ActionResult Details(int id)
        {
            var eventObject = _eventRepository.GetEvent(id);

            if (eventObject == null)
            {
                return HttpNotFound();
            }

            var viewModel = new EventDetailsViewModel { Event = eventObject };

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                viewModel.IsAttending = _attendanceRepository.GetAttendance(userId, eventObject.Id) != null;

                viewModel.IsFollowing = _followingRepository.GetFollowing(userId, eventObject.ArtistId) != null;
            }

            return View("Details", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var eventObject = _eventRepository.GetEvent(id);

            if (eventObject == null)
            {
                return HttpNotFound();
            }

            if (eventObject.ArtistId != User.Identity.GetUserId())
            {
                return new HttpUnauthorizedResult();
            }

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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(EventFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                //we need to initialize and populate Genres prop since viewModel 
                //is new model initialized with values from the httpRequest
                viewModel.Genres = _genreRepository.GetGenres();
                return View("EventForm", viewModel);
            }

            //eager load attendences and atendees
            var eventObject = _eventRepository.GetEventWithAtendees(viewModel.Id);

            if (eventObject == null)
            {
                return HttpNotFound();
            }

            if (eventObject.ArtistId != User.Identity.GetUserId())
            {
                return new HttpUnauthorizedResult();
            }

            eventObject.Modify(viewModel.Venue, viewModel.GetDateTime(), viewModel.Genre);

            _context.SaveChanges();

            return RedirectToAction("Mine", "Events");
        }

        //Action for clicking on Add an Event link in navbar
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new EventFormViewModel
            {
                Genres = _genreRepository.GetGenres(),
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
                viewModel.Genres = _genreRepository.GetGenres();
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
        [HttpGet]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var events = _eventRepository.GetEventsUserAttending(userId);
            var attendances = _attendanceRepository.GetFutureAttendances(userId).ToLookup(a => a.EventId);

            var viewModel = new EventsViewModel
            {
                UpcomingEvents = events,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Events I'm attending",
                Attendances = attendances
            };

            return View("Events", viewModel);
        }


        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var events = _eventRepository.GetUpcomingEventsByArtist(userId);

            return View(events);
        }

        [HttpPost]
        public ActionResult Search(EventsViewModel viewModel)
        {
            //here instead of looking the list of events by search param, redirect to Home controller
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }
    }
}