
var EventDetailsController = function(followingService) {
    var followButton;

    //here we subscribe to the click event
    var init = function() {
        $(".js-toggle-follow").click(toggleFollowing);
    }

    var toggleFollowing = function(e) {
        followButton = $(e.target);

        var foloweeId = followButton.attr("data-followee-id");
        if (followButton.hasClass("btn-default"))
            followingService.createFollowing(foloweeId, done, fail);
        else
            followingService.deleteFollowing(foloweeId, done, fail);

    };
    //private methods should be used instead of anonimous functions

    var done = function () {
        var text = followButton.text() === "Follow" ? "Following" : "Follow";

        //if we have this class, remove it. Othervise, add it
        followButton.toggleClass("btn-info").toggleClass("btn-default").text(text);
    };

    var fail = function () {
        alert("Something failed!");
    };

    return {
        init: init
    }

}(FollowingService);