$(document).ready(function () {
    // Swal.fire({
    //     title: 'Error!',
    //     text: 'Do you want to continue',
    //     icon: 'error',
    //     confirmButtonText: 'Cool'
    //   })
 $('#basic-alert').on('click', function () {
     Swal.fire("Here's a message!");
 });

 $('#with-title').on('click', function () {
     Swal.fire(
         'The Internet?',
         'That thing is still around?'
     );
 });

 $('#with-html').on('click', function () {
     Swal.fire({
         title: 'HTML <small>Title</small>!',
         text: 'A custom <span style="color:#F6BB42">html<span> message.',
         html: true,
         buttonsStyling: false,
         confirmButtonClass: 'btn btn-lg btn-primary'
     });
 });

 $('#alert-success').on('click', function () {
    Swal.fire({
        icon: 'success',
        title: 'Your work has been saved',
        showConfirmButton: false,
        timer: 1500
      })
 });

 $('#alert-info').on('click', function () {
    Swal.fire({
        title: '<strong>HTML <u>example</u></strong>',
        icon: 'info',
        html:
          'You can use <b>bold text</b>, ' +
          '<a href="//sweetalert2.github.io">links</a> ' +
          'and other HTML tags',
        showCloseButton: true,
        showCancelButton: true,
        focusConfirm: false,
        confirmButtonText:
          '<i class="fa fa-thumbs-up"></i> Great!',
        confirmButtonAriaLabel: 'Thumbs up, great!',
        cancelButtonText:
          '<i class="fa fa-thumbs-down"></i>',
        cancelButtonAriaLabel: 'Thumbs down'
      })
 });

 

 $('#alert-error').on('click', function () {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Something went wrong!',
        footer: '<a href>Why do I have this issue?</a>'
      })
 });

 $('#custom-position').on('click', function () {
    Swal.fire({
        position: 'top-end',
        icon: 'success',
        title: 'Your work has been saved',
        showConfirmButton: false,
        timer: 1500
      })
 });

 $("#delete-confirm").on("click", function(){
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.value) {
        Swal.fire(
          "Deleted!",
          "Your file has been deleted.",
          "success"
        )
      }
    })
 });
 $("#chaining").on("click", function(){
  Swal.mixin({
    input: "text",
    confirmButtonText:"Next &rarr;",
    showCancelButton: true,
    progressSteps: ["1", "2", "3"]
  }).queue([
    {
      title: "Question 1",
      text: "Chaining swal2 modals is easy"
    },
    "Question 2",
    "Question 3"
  ]).then((result) => {
    if (result.value) {
      const answers = JSON.stringify(result.value)
      Swal.fire({
        title: "All done!",
        html: `
          Your answers:
          <pre><code>${answers}</code></pre>
        `,
        confirmButtonText: "Lovely!"
      })
    }
  })
 });

 

})