window.onload = function() {
    var timeOptions = [];

    for (var hours = 0; hours <= 23; hours++) {
        for (var minutes = 0; minutes < 60 ; minutes += 5) {
            timeOptions.push(hours + ":" + minutes);
        }
    }
    var dateTimeOptions = {
        allowTimes: timeOptions,
        defaultSelect: true,
        startDate: new Date(),
        minDate: "2000/01/01",
        maxDate: "2020/01/01"
        
    };
    $("#BeginTime").datetimepicker(dateTimeOptions);
    $("#EndTime").datetimepicker(dateTimeOptions);

    $("#createEvent form:first").on("submit", function() {
        $("#createEvent").modal("hide");
    });
}