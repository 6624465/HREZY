function initFullCalendar() {
    function addEvent(name, date, index) {
        var html = '<li data-index="' + index + '"><input class="event-name" value="' + name + '"><div class="event-date">' + date + '</div><div class="event-remove-icon ion ion-trash-a removeEventBtn"></div></li>'; $("#calevents").append(html)
    }
    function removeEvent(calEvent) {
        calEvent.remove(), FC.fullCalendar("removeEvents", function (eventObject) {
            return eventObject.title === calEvent.find(".event-name").attr("value")
        })
    } function changeView(view)
    { FC.fullCalendar("changeView", view), calToolbar.find(".current-date").html(FC.fullCalendar("getView").title) }
    function prev() {
        FC.fullCalendar("prev"), calToolbar.find(".current-date").html(FC.fullCalendar("getView").title)
    }
    function next() {
        FC.fullCalendar("next"), calToolbar.find(".current-date").html(FC.fullCalendar("getView").title)
    }
    function today() {
        FC.fullCalendar("today"), calToolbar.find(".current-date").html(FC.fullCalendar("getView").title)
    }
 

    calEvents = obj,
        FC = $("#fullCalendar").fullCalendar(
    {
        height: 480, editable: !0, defaultView: "month", header: !1, eventLimit: !0, events: calEvents
    }),
calToolbar = $(".calendar-toolbar");
    calToolbar.find(".current-date").html(moment().format("MMMM YYYY")),
    calToolbar.find(".prev-btn").on("click touchstart", prev),
    calToolbar.find(".next-btn").on("click touchstart", next),
    calToolbar.find(".today-btn").on("click touchstart", today),
    calToolbar.find(".view-month").on("click touhstart", function () {
        changeView("month")
    }),
    calToolbar.find(".view-week").on("click touhstart", function () {
        changeView("agendaWeek")
    }),
    calToolbar.find(".view-day").on("click touhstart", function () {
        changeView("agendaDay")
    })
} function _init() { initFullCalendar() } _init()
