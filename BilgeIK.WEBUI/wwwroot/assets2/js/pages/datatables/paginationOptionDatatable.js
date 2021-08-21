"use_strict";

var DatatablePaginationOption = (function(){

  var initDatatable1 = function(){
    var table = $('#paginationOptionDatatable');

    table.DataTable({
      pagingType: "full_numbers",
      responsive:true,
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
	DatatablePaginationOption.init();
});


