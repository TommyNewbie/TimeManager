var eventTemplate;
var errorTemplate;

function formatCurrentDate() {
    return moment().format("YYYY/MM/DD hh:mm");
}

function onSuccess(json) {
    if (json.isValid === true) {
        displayEvents("#future", json.futureEvents);
        displayEvents("#past", json.pastEvents);
        switchTabs();
    } else {
        //display errors
        var res = Mustache.render(errorTemplate, { errors: json.errors });
        $("#content").append(res);
    }
}

function switchTabs() {
    var past$ = $("#past");
    var pastLi$ = $("a[href='#past']").closest("li");
    if (pastLi$.hasClass("active")) {
        past$.addClass("active");
        $("#future").removeClass("active");
    }
}

function displayEvents(elementId, eventsArray) {
    var events = [];

    for (var i = 0; i < eventsArray.length; i++) {
        var value = eventsArray[i];
        var begin = moment(value.beginDate, "MM/DD/YYYY hh:mm");
        var end = moment(value.endDate, "MM/DD/YYYY hh:mm");

        events.push({
            beginDate: begin.format("DD/MM/YYYY"),
            beginTime: begin.format("hh:mm"),
            endDate: end.format("DD/MM/YYYY"),
            endTime: end.format("hh:mm"),
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

    var dateTimeOptions = {
        step: 5,
        defaultSelect: true,
        minDate: "2000/01/01",
        maxDate: "2020/01/01"
    };

    $("#BeginTime").val(formatCurrentDate()).datetimepicker(dateTimeOptions);
    $("#EndTime").val(formatCurrentDate()).datetimepicker(dateTimeOptions);

    //event delition logic
    $("#events").on("click", ".btnDelete", function (e) {
        $.ajax({
            url: "/Events/DeleteEvent",
            data: "id=" + $(this).data("id"),
            method: "POST",
            success: function (json) {
                displayEvents("#future", json.futureEvents);
                displayEvents("#past", json.pastEvents);
            }
        });
    });

    //hide createevent modal
    $("#createEvent form:first").on("submit", function () {
        if ($("#createEvent form").valid()) {
            $("#createEvent").modal("hide");
        }
    });

    //Load events AJAX
    $.ajax({
        url: "/Events/GetEvents",
        success: function (json) {
            displayEvents("#future", json.futureEvents);
            displayEvents("#past", json.pastEvents);
        }
    });
}