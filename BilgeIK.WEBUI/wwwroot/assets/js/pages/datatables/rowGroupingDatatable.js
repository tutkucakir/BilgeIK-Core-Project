"use_strict";

var DatatableBasic = (function(){

  var initDatatable1 = function(){
    var table = $('#rowGroupingDatatable');
    
    table.DataTable({
      responsive:true,
      order: [[2, 'asc']],
      drawCallback: function(settings) {
        var api = this.api();
        var rows = api.rows({page: 'current'}).nodes();
        var last = null;

        api.column(2, {page: 'current'}).data().each(function(group, i) {
          if (last !== group) {
            $(rows).eq(i).before(
              '<tr class="ul-datatable-rowgrouping"><td colspan="10">' + group + '</td></tr>',
            );
            last = group;
          }
        });
      },
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
	DatatableBasic.init();
});
