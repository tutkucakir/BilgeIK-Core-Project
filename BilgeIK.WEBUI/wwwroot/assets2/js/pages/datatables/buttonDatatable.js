"use_strict";

var DatatableButton = (function(){

  var initDatatable1 = function(){
    
    var table = $('#datatableButton').DataTable({
      responsive:true,
      "language": {
        "paginate": {
          "first": `<i style="transform:rotate(180deg)" class="material-icons">double_arrow</i>`,
          "next": `<i class="material-icons">keyboard_arrow_right</i>`,
          "previous": `<i class="material-icons">keyboard_arrow_left</i>`,
          "last": `<i class="material-icons">double_arrow</i>`
        }
      },
      lengthChange: false,
      buttons: [ 
        {
          'extend': 'copy',
          'className': 'btn btn-opacity btn-secondary'
        },
        {
          'extend': 'excel',
          'className': 'btn btn-opacity btn-secondary'
        },
        {
          'extend': 'print',
          'className': 'btn btn-opacity btn-secondary'
        }
      ]
    });   

    table.buttons().container()
         .appendTo( '#datatableButton_wrapper .col-md-6:eq(0)' );
    
  }
  return{
    init: function(){
      initDatatable1();
    }
  }

})();

jQuery(document).ready(function() {
	DatatableButton.init();
});


