"use_strict";

var Scrumboard = (function () {
  var initScrumboard = function () {
    dragula([
      document.getElementById("left-defaults"),
      document.getElementById("right-defaults"),
      ,
      document.getElementById("right-2-defaults"),
    ])
      .on("drag", function (el) {
        // console.log(el);
        el.className += " el-drag-ex-1";
      })
      .on("drop", function (el) {
        // console.log(el);
        el.className = el.className.replace("el-drag-ex-1", "");
      })
      .on("cancel", function (el) {
        // console.log(el);
        el.className = el.className.replace("el-drag-ex-1", "");
      });
  };
  return {
    init: function () {
      initScrumboard();
    },
  };
})();

jQuery(document).ready(function () {
  Scrumboard.init();
});
