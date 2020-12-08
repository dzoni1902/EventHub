using EventHub.Models;
using EventHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
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
        public ActionResult Create(EventFormViewModel viewModel)
        {
            var eventObject = new Event
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = DateTime.Parse(string.Format($"{viewModel.Date}-{viewModel.Time}")),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _context.Events.Add(eventObject);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}