"use strict"

var DatatableScroll = (function(){

  var initTable1 = function(){
    var table = $('#datatableScrollY');

    table.DataTable({
      scrollY: 400,
      responsive:true,
      "language": {
        "paginate": {
          "first": `<i style="transform:rotate(180deg)" class="material-icons">double_arrow</i>`,
          "next": `<i class="material-icons">keyboard_arrow_right</i>`,
          "previous": `<i class="material-icons">keyboard_arrow_left</i>`,
          "last": `<i class="material-icons">double_arrow</i>`
        }
      }
    })

  }

  var initTable2 = function(){
    var table = $('#datatableScrollXY');

    table.DataTable({
      scrollY: 400,
      scrollX: true,
      "language": {
        "paginate": {
          "first": `<i style="transform:rotate(180deg)" class="material-icons">double_arrow</i>`,
          "next": `<i class="material-icons">keyboard_arrow_right</i>`,
          "previous": `<i class="material-icons">keyboard_arrow_left</i>`,
          "last": `<i class="material-icons">double_arrow</i>`
        }
      }
    })
  }

  return{
    init: function(){
      initTable1();
      initTable2();
    },
  }

})()

jQuery(document).ready(function() {
	DatatableScroll.init()
});