"use_strict";

var DatatableJavascript = (function(){

  var initDatatable1 = function(){
    var table = $('#javascriptDatatable');

    table.DataTable({
      responsive:true,
      ajax: '../../assets/js/data/datatable.json',
        columns: [
            { data: 'id' },
            { data: 'name' },
            { data: 'position' },
            { data: 'office' },
            { data: 'extn' },
            { data: 'start_date' },
            { data: 'salary' }
        ],
        rowReorder: {
            dataSrc: 'id'
        },
      "language": {
        "paginate": {
          "first": `<i style="transform:rotate(180deg)" class="material-icons">double_arrow</i>`,
          "next": `<i class="material-icons">keyboard_arrow_right</i>`,
          "previous": `<i class="material-icons">keyboard_arrow_left</i>`,
          "last": `<i class="material-icons">double_arrow</i>`
        }
      },
      
    });    
  }
  return{
    init: function(){
      initDatatable1();
    }
  }

})();

jQuery(document).ready(function() {
	DatatableJavascript.init();
});


