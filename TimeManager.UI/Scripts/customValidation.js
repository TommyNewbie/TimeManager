/// <reference path="jquery-1.9.1.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="moment.js" />

$.validator.unobtrusive.adapters.addSingleVal("startsafter", "date");

$.validator.addMethod("startsafter", function (value, element, data) {
    var today = moment(data, "MM/DD/YYYY HH:mm");
    var value = moment(value, "YYYY/MM/DD HH:mm");
    return today < value;
});

$.validator.unobtrusive.adapters.addSingleVal("morethen", "id");

$.validator.addMethod("morethen", function (value, element, id) {
    var beginTime$ = $(element).closest("form").find("input[name="+id+"]");
    var beginDate = moment(beginTime$.val(), "YYYY/MM/DD HH:mm");
    var endDate = moment(value, "YYYY/MM/DD HH:mm");
    return beginDate < endDate;
});

$.validator.setDefaults({
    highlight: function (element) {
        $(element).closest(".form-group").addClass("has-error");
    },
    unhighlight: function (element) {
        $(element).closest(".form-group").removeClass("has-error");
    }
});
