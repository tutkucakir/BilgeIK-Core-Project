$('input.basicMaxlength').maxlength({
   appendToParent:true,
});


$('input.maxlengthOne').maxlength({
 appendToParent:true,
 threshold: 5,
 warningClass: "badge badge-primary",
 limitReachedClass: "badge badge-danger"
});

$('input.maxlengthTwo').maxlength({
  appendToParent:true,
  alwaysShow: true,
  threshold: 10,
  warningClass: "badge badge-success",
  limitReachedClass: "badge badge-danger",
  separator: ' of ',
  preText: 'You have ',
  postText: ' chars remaining.',
  validate: true
});

$('input.maxlengthL').maxlength({
  appendToParent:true,
  alwaysShow: true,
  placement: 'left',
  warningClass: "badge badge-primary",
  limitReachedClass: "badge badge-danger"
});


