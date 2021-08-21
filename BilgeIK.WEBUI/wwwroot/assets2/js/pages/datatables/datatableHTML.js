"use_strict";

var DatatableHTML = (function(){

  var initDatatable1 = function(){
    var table = $('#basicDatatable');

    table.DataTable({
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
	DatatableHTML.init();
});


