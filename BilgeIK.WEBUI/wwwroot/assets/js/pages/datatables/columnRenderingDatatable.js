"use_strict";

var DatatableColumnRendering = (function(){

  var initDatatable1 = function(){
    var table = $('#columnRenderingDatatable');

    table.DataTable({
      responsive:true,
      "columnDefs": [
        {
            // The `data` parameter refers to the data for the cell (defined by the
            // `data` option, which defaults to the column being worked with, in
            // this case `data: 0`.
            "render": function ( data, type, row ) {
                return data +' ('+ row[3]+')';
            },
            "targets": 0
        },
        { "visible": false,  "targets": [ 3 ] }
      ],
      "language": {
        "paginate": {
          "first": `<i style="transform:rotate(180deg)" class="material-icons">double_arrow</i>`,
          "next": `<i class="material-icons">keyboard_arrow_right</i>`,
          "previous": `<i class="material-icons">keyboard_arrow_left</i>`,
          "last": `<i class="material-icons">double_arrow</i>`
        }
      }
    });    
  }
  return{
    init: function(){
      initDatatable1();
    }
  }

})();

jQuery(document).ready(function() {
	DatatableColumnRendering.init();
});


