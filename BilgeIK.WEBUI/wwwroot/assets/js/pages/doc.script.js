$(document).ready(function() {
  var $examples = $(".doc-example");
  var $docSectionTitle = $('.doc-section-title');
  
  // hilight
  var $codes = $(".code");
  hljs.configure({ useBR: true, tabReplace: "  " });

  $codes.each(function(e) {
    var $code = $(this);
    var code = $code.data("code");

    var highlighted = hljs.highlightAuto(code, ['xml', 'css', 'JavaScript']);
    $code.addClass("hljs");
    $code.html(hljs.fixMarkup(highlighted.value));
    $code.wrap("<pre class='collapsed'></pre>");
  });

  $('pre.collapsed').on('click', function(e) {
    $(this).removeClass('collapsed');    
  });

  // Add border line to title
  $docSectionTitle.append('<span class="border-bottom"></span>');

  // // Create and add id to title
  // $docSectionTitle.each(function(e) {
  //   var $title = $(this);
  //   var id = $title.text().replace(/\W/g,'_');
  //   $title.attr('id', id);
  //   $title.find('.section-link').attr('href', '#'+id);
  //   console.log($title.text());
  // });  
  

  // copy to clipboard
  var clipboard = new ClipboardJS(".btn-clipboard", {
    target: function(trigger) {
      return trigger.parentNode.nextElementSibling;
    }
  });

  clipboard.on("success", function(e) {
    $(e.trigger)
      .attr("title", "Copied!")
      .tooltip("_fixTitle")
      .tooltip("show")
      .attr("title", "Copy to clipboard")
      .tooltip("_fixTitle");

    e.clearSelection();
  });

  clipboard.on("error", function(e) {});

});