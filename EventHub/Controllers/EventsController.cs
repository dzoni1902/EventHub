using EventHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using EventHub.Core;
using EventHub.Core.Models;
using EventHub.Core.ViewModels;

namespace EventHub.Controllers
{
    public class EventsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public ActionResult Details(int id)
        {
            var eventObject = _unitOfWork.Events.GetEvent(id);

            if (eventObject == null)
            {
                return HttpNotFound();
            }

            var viewModel = new EventDetailsViewModel { Event = eventObject };

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                viewModel.IsAttending = _unitOfWork.Attendances.GetAttendance(userId, eventObject.Id) != null;

                viewModel.IsFollowing = _unitOfWork.Followings.GetFollowing(userId, eventObject.ArtistId) != null;
            }

            return View("Details", viewModel);
        }

        //Edit & Create actions take us to page where we CREATE new or EDIT existing event
        //these actions return VIEW's, GET actions

        [Authorize]
        public ActionResult Edit(int id)
        {
            var eventObject = _unitOfWork.Events.GetEvent(id);

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
                Genres = new ApplicationDbContext().Genres.ToList(),
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
                viewModel.Genres = _unitOfWork.Genres.GetGenres();
                return View("EventForm", viewModel);
            }

            //eager load attendences and atendees
            var eventObject = _unitOfWork.Events.GetEventWithAtendees(viewModel.Id);

            if (eventObject == null)
            {
                return HttpNotFound();
            }

            if (eventObject.ArtistId != User.Identity.GetUserId())
            {
                return new HttpUnauthorizedResult();
            }

            eventObject.Modify(viewModel.Venue, viewModel.GetDateTime(), viewModel.Genre);

            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Events");
        }

        //Action for clicking on Add an Event link in navbar
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new EventFormViewModel
            {
                Genres = _unitOfWork.Genres.GetGenres(),
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
                viewModel.Genres = _unitOfWork.Genres.GetGenres();
                return View("EventForm", viewModel);
            }

            var eventObject = new Event
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _unitOfWork.Events.Add(eventObject);
            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Events");
        }


        [Authorize]
        [HttpGet]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var events = _unitOfWork.Events.GetEventsUserAttending(userId);
            var attendances = _unitOfWork.Attendances.GetFutureAttendances(userId).ToLookup(a => a.EventId);

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
            var events = _unitOfWork.Events.GetUpcomingEventsByArtist(userId);

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