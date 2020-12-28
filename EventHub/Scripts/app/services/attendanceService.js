
//FE controller is all about handling the events raised from the view and updating the view
//NOT data access manipulation
var AttendanceService = function () {
    var createAttendance = function (eventId, done, fail) {
        $.post("/api/attendances", { EventId: eventId })
            .done(done)
            .fail(fail);
    };

    var deleteAttendance = function (eventId, done, fail) {
        $.ajax({
                url: "/api/attendances/" + eventId,
                method: "DELETE"
            })
            .done(done)
            .fail(fail);
    };

    return {
        createAttendance: createAttendance,
        deleteAttendance: deleteAttendance
    }

}();    //IIFE