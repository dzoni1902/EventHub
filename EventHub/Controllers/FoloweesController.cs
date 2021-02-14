using EventHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using EventHub.Core;

namespace EventHub.Controllers
{
    public class FoloweesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoloweesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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