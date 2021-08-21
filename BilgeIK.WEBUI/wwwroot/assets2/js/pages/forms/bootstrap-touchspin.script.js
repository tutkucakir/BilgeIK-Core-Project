$("input[name='demo1']").TouchSpin({
  min: 0,
  max: 100,
  step: 0.1,
  decimals: 2,
  boostat: 5,
  maxboostedstep: 10,
});


$("input[name='demo2']").TouchSpin({
    min: -1000000000,
    max: 1000000000,
    stepinterval: 50,
    maxboostedstep: 10000000,
    prefix: '$'
});

$("input[name='demo3']").TouchSpin({
    min: -1000000000,
    max: 1000000000,
    stepinterval: 50,
    maxboostedstep: 10000000,
    postfix: '$'
});


$("input[name='demo_vertical']").TouchSpin({
  verticalbuttons: true
});

$("input[name='demo_vertical2']").TouchSpin({
  buttondown_class: 'btn btn-secondary',
  buttonup_class: 'btn btn-secondary',
  verticalbuttons: true,
  verticalupclass: 'fas fa-chevron-up',
  verticaldownclass: 'fas fa-chevron-down'
});
