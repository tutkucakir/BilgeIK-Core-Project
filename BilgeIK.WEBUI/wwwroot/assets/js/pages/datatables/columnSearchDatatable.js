"use_strict";

var DatatableColumnSearch = (function(){

  var initDatatable1 = function(){

    // Setup - add a text input to each header cell
    $('#columnSearchDatatable thead tr:eq(1) th').each( function () {
      var title = $(this).text();
      $(this).html( '<input type="text" placeholder="Search '+title+'" class="form-control" />' );
    });

    

    

    var table = $('#columnSearchDatatable').DataTable({
      responsive:true,
      "language": {
        "paginate": {
          "first": `<i style="transform:rotate(180deg)" class="material-icons">double_arrow</i>`,
          "next": `<i class="material-icons">keyboard_arrow_right</i>`,
          "previous": `<i class="material-icons">keyboard_arrow_left</i>`,
          "last": `<i class="material-icons">double_arrow</i>`
        }
      },
      orderCellsTop: true,
      fixedHeader: true,
     
        
    });  
    
    $( '#columnSearchDatatable thead'  ).on( 'keyup', ".form-control",function () {
 
      table
          .column( $(this).parent().index() )
          .search( this.value )
          .draw();
          console.log('asdoijui');
          
    } );


  }
  return{
    init: function(){
      initDatatable1();
    }
  }

})();

jQuery(document).ready(function() {
	DatatableColumnSearch.init();
});


