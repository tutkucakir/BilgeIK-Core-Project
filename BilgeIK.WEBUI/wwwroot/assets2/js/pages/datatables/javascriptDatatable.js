"use_strict";

var DatatableJavascript = (function(){
  var data = [
      [
          "Tiger Nixon",
          "System Architect",
          "Edinburgh",
          "5421",
          "2011/04/25",
          "$3,120"
      ],
      [
          "Garrett Winters",
          "Director",
          "Edinburgh",
          "8422",
          "2011/07/25",
          "$5,300"
      ]
  ]
  var initDatatable1 = function(){
    var table = $('#javascriptDatatable');

    table.DataTable({
      responsive:true,
      data: data,   
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


