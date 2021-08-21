"use_strict";

var DatatableBasic = (function(){

  var initDatatable1 = function(){
    var table = $('#datatableColumnSelect');

    
    
    table.DataTable({
      responsive:true,
      "language": {
        "paginate": {
          "first": `<i style="transform:rotate(180deg)" class="material-icons">double_arrow</i>`,
          "next": `<i class="material-icons">keyboard_arrow_right</i>`,
          "previous": `<i class="material-icons">keyboard_arrow_left</i>`,
          "last": `<i class="material-icons">double_arrow</i>`
        }
      },
      initComplete: function () {
        this.api().columns().every( function () {
            var column = this;
            var select = $('<select class="selectpicker" data-style="btn-raised-default"><option value=""></option></select>')
                .appendTo( $(column.footer()).empty() )
                .on( 'change', function () {
                    var val = $.fn.dataTable.util.escapeRegex(
                        $(this).val()
                    );

                    column
                        .search( val ? '^'+val+'$' : '', true, false )
                        .draw();
                } );

            column.data().unique().sort().each( function ( d, j ) {
                select.append( '<option value="'+d+'">'+d+'</option>' )
            } );
        } );
        $.fn.selectpicker.Constructor.BootstrapVersion = '4';
        $('.selectpicker').selectpicker();
        
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


