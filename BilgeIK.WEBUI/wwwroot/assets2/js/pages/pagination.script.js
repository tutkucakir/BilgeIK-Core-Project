"use_strict";

var PaginationTable = (function(){

  var initPagination = function(){
    function template(data) {
      var html = '<ul>';
      $.each(data, function(index, item){
          html += '<li>'+ item +'</li>';
      });
      html += '</ul>';
      return html;
    }
    
    function log(content) {
      window.console && console.log(content);
    }
    
    $(function(){
      var container = $('#pagination-bar');
    
      
      container.pagination({
          dataSource: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
          pageSize: 2,
          autoHidePrevious: true,
          autoHideNext: true,
          callback: function(data, pagination) {
            var html = template(data);
            $('#pagination-data-container').html(html);
          }
      });
      
    });
  }
  return{
    init: function(){
      initPagination();
    }
  }

})();

jQuery(document).ready(function() {
	PaginationTable.init();
});


