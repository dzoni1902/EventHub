using EventHub.Models;
using EventHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace EventHub.Controllers
{
    public class FoloweesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public FoloweesController()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
        }

        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var artists = _unitOfWork.Followings.GetArtistsIFollow(userId);

            return View(artists);
        }
    }
}