﻿using EventHub.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace EventHub.Controllers.WebAPI
{
    [Authorize]
    public class EventsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public EventsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var eventObject = _context.Events
                .Include(e => e.Attendances.Select(a => a.Attendee))
                .Single(e => e.Id == id && e.ArtistId == userId);

            if (eventObject.IsCanceled)
            {
                return NotFound();
            }

            eventObject.CancelEvent();

            _context.SaveChanges();

            return Ok();
        }
    }
}