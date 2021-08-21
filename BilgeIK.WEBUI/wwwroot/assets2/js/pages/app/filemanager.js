"use_strict";

var Filemanager = (function() {
  var initFilemanager = function() {
    
    var menuButton = document.querySelector('.ul-filemanager-menu-button');
    var sidebar = document.querySelector('.ul-sidebar-left');
    var overlay = document.querySelector('.ul-sidebar-overlay');

    menuButton.addEventListener('click', () => {
      if(sidebar){
        ULUtil.hasClass(sidebar, 'open') ? ULUtil.removeClass(sidebar, 'open') : ULUtil.addClass(sidebar, 'open')
      }
      if(overlay){
        ULUtil.hasClass(overlay, 'open') ? ULUtil.removeClass(overlay, 'open') : ULUtil.addClass(overlay, 'open')
      }
      
    })

    overlay.addEventListener('click', () => {
      if(overlay){
        ULUtil.hasClass(sidebar, 'open') ? ULUtil.removeClass(sidebar, 'open') : ULUtil.addClass(sidebar, 'open')
        ULUtil.hasClass(overlay, 'open') ? ULUtil.removeClass(overlay, 'open') : ULUtil.addClass(overlay, 'open')
      }
    })
    
   
  };
  return {
    init: function() {
      initFilemanager();
    }
  };
})();

jQuery(document).ready(function() {
  Filemanager.init();
});
