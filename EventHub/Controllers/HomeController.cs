using EventHub.Models;
using EventHub.Repositories;
using EventHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;


namespace EventHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttendanceRepository _attendanceRepository;

        public HomeController()
        {
            _context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(_context);
        }

        public ActionResult Index(string query = null)
        {
            var upcomingEvents = _context.Events
                .Include(e => e.Artist)
                .Include(e => e.Genre)
                .Where(e => e.DateTime > DateTime.Now && !e.IsCanceled)
                .ToList();

            //if there is a query, we need to apply a filter
            if (!String.IsNullOrWhiteSpace(query))
            {
                upcomingEvents = upcomingEvents
                    .Where(e => e.Artist.Name.ToLowerInvariant().Contains(query.ToLowerInvariant())
                                || e.Genre.Name.ToLowerInvariant().Contains(query.ToLowerInvariant())
                                || e.Venue.ToLowerInvariant().Contains(query.ToLowerInvariant()))
                    .ToList();
            }

            var userId = User.Identity.GetUserId();
            var attendances = _attendanceRepository.GetFutureAttendances(userId).ToLookup(a => a.EventId);

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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}