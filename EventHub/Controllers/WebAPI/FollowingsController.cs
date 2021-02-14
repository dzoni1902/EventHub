using EventHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using EventHub.Core;
using EventHub.Core.Dtos;
using EventHub.Core.Models;

namespace EventHub.Controllers.WebAPI
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public FollowingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();
            var followingExists = _unitOfWork.Followings.GetFollowing(userId, dto.FolloweeId) != null;

            if (followingExists)
            {
                return BadRequest("Following already exists.");
            }

            var following = new Following
            {
                FollowerId = userId,
                FolloweeId = dto.FolloweeId

            };

            _unitOfWork.Followings.Add(following);
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Unfollow(string id)
        {
            var userId = User.Identity.GetUserId();
            var following = _unitOfWork.Followings.GetFollowing(userId, id);

            if (following == null)
            {
                return NotFound();
            }

            _unitOfWork.Followings.Remove(following);
            _unitOfWork.Complete();

            return Ok(id);
        }
    }
}
