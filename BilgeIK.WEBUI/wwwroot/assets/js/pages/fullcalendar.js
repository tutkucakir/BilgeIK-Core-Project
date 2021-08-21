
$(document).ready(function () {


  /* initialize the external events
  -----------------------------------------------------------------*/


  function initEvent() {
    $('#external-events .fc-event').each(function () {

      // store data so the calendar knows to render an event upon drop
      $(this).data('event', {
        title: $.trim($(this).text()), // use the element's text as the event title
        color: $(this).css('background-color'),
        stick: true // maintain when user navigates (see docs on the renderEvent method)
      });

      // make the event draggable using jQuery UI
      $(this).draggable({
        zIndex: 999,
        revert: true, // will cause the event to go back to its
        revertDuration: 0 // original position after the drag
      });

    });
  }
  initEvent();


  /* initialize the calendar
  -----------------------------------------------------------------*/
  var newDate = new Date,
    date = newDate.getDate(),
    month = newDate.getMonth(),
    year = newDate.getFullYear();
  // var time = newDate.getHours() + ":" + newDate.getMinutes() + ":" + newDate.getSeconds();
  $('#calendar').fullCalendar({
    
    header: {
      left: 'prev,next today',
      center: 'month,agendaWeek,agendaDay ',
      right: ' title'
    },
    themeSystem: "bootstrap4",
    droppable: true,
    editable: true,
    eventLimit: true, // allow "more" link when too many events
    drop: function () {
      // is the "remove after drop" checkbox checked?
      if ($('#drop-remove').is(':checked')) {
        // if so, remove the element from the "Draggable Events" list
        $(this).remove();
      }
    },
    selectable: true,
    selectHelper: true,
    select: function (start, end) {
      // Display the modal.
      // You could fill in the start and end fields based on the parameters

      //it was 'show' instead of hide --R
      $('.modal').modal('show');

    },
    eventClick: function (event, element) {
      // Display the modal and set the values to the event values.
      
      $('.modal').modal('show');
      $('.modal').find('#title').val(event.title);
      $('.modal').find('#starts-at').val(event.start);
      $('.modal').find('#ends-at').val(event.end);

    },

    events: [{
      title: "Break time",
      start: new Date(year, month, 1),
      allDay: !0,
      color: "#ffc107"
    }, {
      title: "Office Hour",
      start: new Date(year, month, 3)
    }, {
      title: "Work on a Project",
      start: new Date(year, month, 9),
      end: new Date(year, month, 12),
      allDay: !0,
      color: "#d22346"
    }, {
      title: "Work on a Project",
      start: new Date(year, month, 17),
      end: new Date(year, month, 19),
      allDay: !0,
      color: "#d22346"
    }, {
      id: 999,
      title: "Go to Long Drive",
      start: new Date(year, month, date - 1, 15, 0)
    }, {
      id: 999,
      title: "Go to Long Drive",
      start: new Date(year, month, date + 3, 15, 0)
    }, {
      title: "Work on a New Project",
      start: new Date(year, month, date - 3),
      end: new Date(year, month, date - 3),
      allDay: !0,
      color: "#ffc107"
    }, {
      title: "Food ",
      start: new Date(year, month, date + 7, 15, 0),
      color: "#4caf50"
    }, {
      title: "Go to Library",
      start: new Date(year, month, date, 8, 0),
      end: new Date(year, month, date, 14, 0),
      color: "#ffc107"
    }, {
      title: "Go for Walk",
      start: new Date(year, month, 25),
      end: new Date(year, month, 27),
      allDay: !0,
      color: "#ffc107"
    }, {
      title: "Work on a Project",
      start: new Date(year, month, date + 8, 20, 0),
      end: new Date(year, month, date + 8, 22, 0)
    }]
  });








  jQuery(".js-form-add-event").on("submit", function (e) {
    e.preventDefault();

    var data = $('#newEvent').val();
    $('#newEvent').val('');
    $('#external-events').prepend('<li class="list-group-item bg-success fc-event mt-md">' + data + '</li>');

    initEvent();
  });
  // Bind the dates to datetimepicker.
  // You should pass the options you need
  $("#starts-at, #ends-at").pickadate();

  // Whenever the user clicks on the "save" button om the dialog
  $('#save-event').on('click', function () {
    var title = $('#title').val();
    if (title) {
      var eventData = {
        title: title,
        start: $('#starts-at').val() ,
        end: $('#ends-at').val() 
      };
      $('#calendar').fullCalendar('renderEvent', eventData, true); // stick? = true
    }
    $('#calendar').fullCalendar('unselect');

    // Clear modal inputs
    $('.modal').find('input').val('');

    // hide modal
    $('.modal').modal('hide');
  });

});

