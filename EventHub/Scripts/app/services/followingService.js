
//FE controller is all about handling the events raised from the view and updating the view
//NOT data access manipulation, thats why we need this service
var FollowingService = function() {
    var createFollowing = function(followeeId, done, fail) {
        $.post("/api/followings", { FolloweeId: followeeId })
            .done(done)
            .fail(fail);
    };

    var deleteFollowing = function(followeeId, done, fail) {
        $.ajax({
                url: "/api/followings/" + followeeId,
                method: "DELETE"
            })
            .done(done)
            .fail(fail);
    };

    return {
        createFollowing: createFollowing,
        deleteFollowing: deleteFollowing
    }

}();
