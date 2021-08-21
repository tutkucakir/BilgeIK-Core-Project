$(document).ready(function () {
  // Collapsable Sidebar
  var $sidebarContainer = $('[data-sidebar-container]')
      .addClass('sidebar-container');
  var $sidebarContent = $('[data-sidebar-content]')
      .addClass('sidebar-content');
  var $sidebar = $('[data-sidebar]')
      .addClass('sidebar');
  var $overlay = $('.sidebar-overlay');

  $sidebarContainer.each(function (index) {
      var $container = $(this);
      var sidebarName;
      var $content;
      var $sb;
      var $toggle;
      var sidebarWidth;
      var sidebarPosition;

      function initSidebar() {
          sidebarName = $container.data('sidebar-container');
          $content = $('[data-sidebar-content="' + sidebarName + '"]');
          $sb = $('[data-sidebar="' + sidebarName + '"]');
          $toggle = $('[data-sidebar-toggle="' + sidebarName + '"]');
          sidebarWidth = $sb.outerWidth();
          sidebarPosition = $sb.data('sidebar-position');
          // console.log('width:: ', $sb.outerWidth());

          if (sidebarPosition === 'right') {
              !ULUtil.isMobile() ? $content.css('margin-right', sidebarWidth) : $content.css('margin-right', 0);
              !ULUtil.isMobile() ? $sb.css('right', 0) : $sb.css('right', -sidebarWidth);
          } else {
              // console.log('width: ', sidebarWidth)
              !ULUtil.isMobile() ? $content.css('margin-left', sidebarWidth) : $content.css('margin-left', 0);
              !ULUtil.isMobile() ? $sb.css('left', 0) : $sb.css('left', -sidebarWidth);
          }
      }

      initSidebar();
      $(window).on("resize", function (event) {
          setTimeout(function () {
              initSidebar();
          }, 300)
      });

      function closeSidebar (e) {
        var isMobile = ULUtil.isMobile();
        console.log('close');
          $overlay.removeClass('open');
          if (sidebarPosition === 'right') {
              if ($sb.css('right') == '0px') {
                  $sb.css('right', -sidebarWidth);
                  !isMobile ? $content.css('margin-right', 0) : null;
              }
          } else {
              if ($sb.css('left') == '0px') {
                  $sb.css('left', -sidebarWidth);
                  !isMobile ? $content.css('margin-left', 0) : null;
              }
          }
      }

      function openSidebar(e) {
        var isMobile = ULUtil.isMobile();
        console.log('open');
        
          if (sidebarPosition === 'right') {
             
                  $sb.css('right', 0);
                  !isMobile ? $content.css('margin-right', sidebarWidth) : null;
                  isMobile ? $overlay.addClass('open') : $overlay.removeClass('open');
          } else {
              
                  $sb.css('left', 0);
                  !isMobile ? $content.css('margin-left', sidebarWidth) : null;
                  isMobile ? $overlay.addClass('open') : $overlay.removeClass('open');
              
          }
      }

      function toggleSidebar (e) {
        var isMobile = ULUtil.isMobile();
        console.log('toggle');
          if ($sb.css('right') == '0px' || $sb.css('left') == '0px') {
            closeSidebar()
          } else {
            openSidebar();
          }
          // if (sidebarPosition === 'right') {
          //     if ($sb.css('right') == '0px') {
          //         $sb.css('right', -sidebarWidth);
          //         !isMobile ? $content.css('margin-right', 0) : null;
          //     } else {
          //         $sb.css('right', 0);
          //         !isMobile ? $content.css('margin-right', sidebarWidth) : null;
          //     }
          // } else {
          //     if ($sb.css('left') == '0px') {
          //         $sb.css('left', -sidebarWidth);
          //         !isMobile ? $content.css('margin-left', 0) : null;
          //     } else {
          //         $sb.css('left', 0);
          //         !isMobile ? $content.css('margin-left', sidebarWidth) : null;
          //         isMobile ? $overlay.addClass('open') : $overlay.removeClass('open');
          //     }
          // }
      }
      $toggle.on('click', toggleSidebar);
      $overlay.on('click', closeSidebar);
  });
})