$(document).ready(function () {
 $('.js-example-basic-single').select2();
});

$(document).ready(function () {
 $('.js-example-basic-multiple').select2();
});

$('.js-data-example-ajax').select2({
 dropdownParent: $('.main-content-body'),
 ajax: {
  url: 'https://api.github.com/search/repositories',
  dataType: 'json',
  processResults: function (data) {

   var results = $.map(data.items, function (item) {
    return {
     text: item.name,
     id: item.id
    }
   })
   return {results: results}
  }
 }
});

$(document).ready(function () {
 $('.js-example-disabled').select2({
   disabled:true
 });
});