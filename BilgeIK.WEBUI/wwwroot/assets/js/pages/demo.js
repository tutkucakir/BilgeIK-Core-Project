"use_strict";

var EgretDemo = (function () {
  var initParallax = function () {
    var scene = document.getElementById("intro-parallax-layers");
    var parallaxInstance = new Parallax(scene, {
      scalarX: 10.0,
      scalarY: 0.0,
    });
  };

  var initScroll = function () {
    // document.getElementById('');
    var btnDemos = ULUtil.getByID("btn-view-demos");
    var test = ULUtil.get('btn-view-demos');
    console.log(test);

    if(btnDemos)  {
      
      btnDemos.addEventListener('click', (e) => {
        e.preventDefault();
        document
          .getElementById('demos')
          .scrollIntoView({ behavior: 'smooth' });
          
        })
    }

  };

  var initDemo = function () {
    initParallax();
    initScroll();
  };

  return {
    init: function () {
      initDemo();
    },
  };
})();

ULUtil.ready(function () {
  EgretDemo.init();
});
