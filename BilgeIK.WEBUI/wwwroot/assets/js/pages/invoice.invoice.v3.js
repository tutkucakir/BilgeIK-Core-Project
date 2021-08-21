$(document).ready(function()
{
    $(".ul-repeater-invoidce-v3").repeater({
      show:function(){
        $(this).slideDown()
      },
      hide:function(e){
        confirm("Are you sure you want to delete ??????")&&$(this).slideUp(e)}
    })

    $("#ul-invoice-v3-datepicker").daterangepicker({
      singleDatePicker: true,
      parentEl: ".main-content-body",
    });
});