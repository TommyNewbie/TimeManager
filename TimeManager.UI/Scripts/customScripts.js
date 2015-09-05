var eventTemplate;
var errorTemplate;
var intersectionTemplate;

var dateTimeOptions = {
    step: 5,
    validateOnBlur: true,
    minDate: "2000/01/01",
    maxDate: "2020/01/01"
};

//returns current date-time in special format
function formatCurrentDate() {
    return moment().format("YYYY/MM/DD HH:mm");
}

//format date-time from server for date-time input
function formatServerTime(time) {
    return moment(time, "MM/DD/YYYY HH:mm").format("YYYY/MM/DD HH:mm");
}

//onSuccess method for create event form
function onSuccess(json) {
    if (json.status === "valid") {
        displayEvents("#future", json.futureEvents);
        displayEvents("#past", json.pastEvents);
    } else if (json.status === "error") {
        displayErrors(json.errors);
    } else if (json.status === "intersected") {
        var intersectModal$ = $("#intersectionEvent");
        intersectModal$.find("input[name=Id]").val(json.id);
        intersectModal$.find("input[name=Name]").val(json.name);
        intersectModal$.find(".modal-body").html(Mustache.render(intersectionTemplate, json));
        intersectModal$.modal({ show: true });
    }
}

//display errors from json
function displayErrors(errors) {
    var res = Mustache.render(errorTemplate, { errors: errors });
    $("#errors").html(res);
}
//display events from array in element
function displayEvents(elementId, eventsArray) {
    var events = [];

    for (var i = 0; i < eventsArray.length; i++) {
        var value = eventsArray[i];
        var begin = moment(value.beginDate, "MM/DD/YYYY HH:mm");
        var end = moment(value.endDate, "MM/DD/YYYY HH:mm");

        events.push({
            beginDate: begin.format("DD/MM/YYYY"),
            beginTime: begin.format("HH:mm"),
            endDate: end.format("DD/MM/YYYY"),
            endTime: end.format("HH:mm"),
            id: value.id,
            name: value.name
        });
    }

    $(elementId).html(Mustache.render(eventTemplate, { events: events }));
}

window.onload = function () {
    eventTemplate = $("#eventTemplate").html();
    Mustache.parse(eventTemplate);
    errorTemplate = $("#errorTemplate").html();
    Mustache.parse(errorTemplate);
    intersectionTemplate = $("#intersectionTemplate").html();
    Mustache.parse(intersectionTemplate);

    $("#BeginTime").val(formatCurrentDate()).datetimepicker(dateTimeOptions);
    $("#EndTime").val(formatCurrentDate()).datetimepicker(dateTimeOptions);

    //event deletion logic
    $("#events").on("click", ".btnDelete", function (e) {
        $.ajax({
            url: "/Events/Delete",
            data: "id=" + $(this).data("id"),
            method: "POST",
            success: function (json) {
                displayEvents("#future", json.futureEvents);
                displayEvents("#past", json.pastEvents);
            }
        });
    });

    //event update logic
    $("#events").on("click", ".btnEdit", function (e) {
        $.ajax({
            url: "Events/Update",
            method: "GET",
            data: "id=" + $(this).data("id"),
            success: function (json) {
                if (json.status === "valid") {
                    var event = json.entry;
                    var editModal$ = $("#editEvent");
                    editModal$.modal({ show: true });
                    editModal$.find("input[name=Id]").val(event.id)
                    editModal$.find("input[name=Name]").val(event.name);
                    editModal$.find("input[name=BeginTime]").val(formatServerTime(event.beginTime)).datetimepicker(dateTimeOptions);
                    editModal$.find("input[name=EndTime]").val(formatServerTime(event.endTime)).datetimepicker(dateTimeOptions);
                } else if (json.status === "error") {
                    displayErrors(json.errors);
                }
            }
        });
    });

    //hides createEvent modal
    $("#createEvent form:first").on("submit", function () {
        if ($("#createEvent form").valid()) {
            $("#createEvent").modal("hide");
        }
    });

    //hides editEvent modal
    $("#editEvent form:first").on("submit", function () {
        if ($("#editEvent form").valid()) {
            $("#editEvent").modal("hide");
        }
    });

    //hides intersectionEvent modal
    $("#intersectionEvent form:first").on("submit", function () {
        $("#intersectionEvent").modal("hide");
    });

    //Loads events with AJAX
    $.ajax({
        url: "/Events/GetAll",
        method: 'GET',
        success: function (json) {
            displayEvents("#future", json.futureEvents);
            displayEvents("#past", json.pastEvents);
        }
    });
}