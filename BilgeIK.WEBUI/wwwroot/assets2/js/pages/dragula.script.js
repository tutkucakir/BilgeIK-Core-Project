"use_strict";

var Dragula = (function() {
  var initDragula = function() {

    
    dragula([document.getElementById('left-defaults'), document.getElementById('right-defaults')])
      .on('drag', function (el) {
      console.log(el);
      el.className += ' el-drag-ex-1';
      }).on('drop', function (el) {
      console.log(el);
      el.className = el.className.replace('el-drag-ex-1', '');
      }).on('cancel', function (el) {
      console.log(el);
      el.className = el.className.replace('el-drag-ex-1', '');
    });


    // icons-change 

    dragula([document.getElementById("left-events"), document.getElementById("right-events")])
        .on('drag', function (el) {
        console.log(el);
        el.className += ' el-drag-ex-2';
        el.className = el.className.replace('ex-moved', '');
        })
        .on('drop', function (el, target, source, sibling) {
        console.log(el);
        el.className = el.className.replace('el-drag-ex-2', '');
        el.className += ' ex-moved';
        })
        .on('over', function (el, container) {
        container.className += ' ex-over';
        })
        .on('out', function (el, container) {
        container.className = container.className.replace('ex-over', '');
        })
        .on('cancel', function (el) {
        console.log(el);
        el.className = el.className.replace('el-drag-ex-2', '');
    });


    // delete-user
    dragula([document.getElementById("left-remove"), document.getElementById("right-remove")], { removeOnSpill: true })
      .on('drag', function (el) {
      console.log(el);
      el.className += ' el-drag-ex-3';
      }).on('drop', function (el) {
      console.log(el);
      el.className = el.className.replace('el-drag-ex-3', '');
      }).on('cancel', function (el) {
      console.log(el);
      el.className = el.className.replace('el-drag-ex-3', '');
    });

    // news-feed 
    dragula([document.getElementById('left-feed'), document.getElementById('right-feed')])
      .on('drag', function (el) {
      console.log(el);
      el.className += ' el-drag-ex-1';
      }).on('drop', function (el) {
      console.log(el);
      el.className = el.className.replace('el-drag-ex-1', '');
      }).on('cancel', function (el) {
      console.log(el);
      el.className = el.className.replace('el-drag-ex-1', '');
    });
    

    //drag-handler
    dragula([document.getElementById('left-handle'), document.getElementById('right-handle')], {
      
      moves: function (el, container, handle) {
        return handle.classList.contains('handle');
      }
      }).on('drag', function (el) {
      console.log(el);
      el.className += ' el-drag-ex-5';
      }).on('drop', function (el) {
      console.log(el);
      el.className = el.className.replace('el-drag-ex-5', '');
      }).on('cancel', function (el) {
      console.log(el);
      el.className = el.className.replace('el-drag-ex-5', '');
    })

  };
  return {
    init: function() {
      initDragula();
    }
  };
})();

jQuery(document).ready(function() {
  Dragula.init();
});
