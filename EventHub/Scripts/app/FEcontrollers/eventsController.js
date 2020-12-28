
//inside of this module we need a reference to a AttendanceService module
var EventsFEController = function (attendanceService) {

    var button;

    var init = function (container) {
        $(container).on("click", ".js-toggle-attendance", toggleAttendance);
    };

    var toggleAttendance = function (e) {
        //making an ajax call to the API using POST method:
        button = $(e.target);

        var eventId = button.attr("data-event-id");
        //we need to check if attendance already exists
        if (button.hasClass("btn-default"))
            attendanceService.createAttendance(eventId, done, fail);
        else
            attendanceService.deleteAttendance(eventId, done, fail);
    };

    //private methods should be used instead of anonimous functions

    var done = function () {
        var text = button.text() == "Going" ? "Going ?" : "Going";

        //if we have this class, remove it. Othervise, add it
        button.toggleClass("btn-info").toggleClass("btn-default").text(text);
    };

    var fail = function () {
        alert("Something failed!");
    };

    return {
        init: init
    }

    //this is revealing module pattern

}(AttendanceService);    //immediately invoked function, thats why we have to pass the param here

