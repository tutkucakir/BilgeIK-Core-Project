"use_strict";

var Note = (function () {
  var initNote = function () {
    // mobile-sidebar-menu-toggle
    let sidebarButton = document.querySelector(
      ".ul-note-mobile-sidebar button"
    );
    let sidebar = document.querySelector(".ul-note-sidebar");

    sidebarButton.addEventListener("click", () => {
      ULUtil.hasClass(sidebar, "open")
        ? ULUtil.removeClass(sidebar, "open")
        : ULUtil.addClass(sidebar, "open");
    });

    // represents a note
    class Note {
      constructor(title, desc) {
        this.title = title;
        this.description = desc;
      }
    }
    //delete note from the row
    let deleteNote = (el) => {
      if (el.classList.contains("remove-note")) {
        el.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.remove();
      }
    };

    //dom inputs clear
    var clearFields = () => {
      document.querySelector("#title").value = " ";
      document.querySelector("#desc").value = "";
    };
    // submit note
    document.querySelector("#ul-note-form").addEventListener("submit", (e) => {
      e.preventDefault();
      // Get form note
      let title = document.querySelector("#title").value;
      let desc = document.querySelector("#desc").value;

      // instatiate note
      const note = new Note(title, desc);

      clearFields();

      // console.log(note);

      // create UI
      var row = document.querySelector(".row");
      const col = document.createElement("div");
      col.className = "col-xl-4 col-lg-12 mb-md";
      col.innerHTML = `
        <div class="card">
          <div class="card-body">
            <div class="card-title">${note.title}</div>
            <p>${note.description}</p>
            <div class="d-flex align-items-center justify-content-between">
              <div class="ul-note-action">
                <a href="">
                  <i class="material-icons">star_border</i>
                </a>
                <a href="">
                  <i class="material-icons">delete_outline</i>
                </a>
              </div>
              <a href="">
                <i class="material-icons">donut_large</i>
              </a>
            </div>
          </div>
        </div>
        `;

      row.appendChild(col);
    });

    //click remove note-item
    document
      .querySelector(".ul-note-container")
      .addEventListener("click", (e) => {
        e.preventDefault();
        // console.log(e.target);
        deleteNote(e.target);
      });

    //toggle note-favorite
    var favoriteActions = document.querySelectorAll(".ul-fav-note");
    var noteItem = document.querySelectorAll(".ul-note-item");
    favoriteActions.forEach((el) => {
      el.addEventListener("click", (e) => {
        var favItem = e.target;

        if (favItem) {
          // ULUtil.addClass(noteItem, 'ul-note-fav');
          noteItem.forEach((item) => {
            var perItem = item;
            // console.log(perItem);

            ULUtil.hasClass(perItem, "ul-note-fav")
              ? ULUtil.removeClass(perItem, "ul-note-fav")
              : ULUtil.addClass(perItem, "ul-note-fav");
          });
        }
      });
    });
  };
  return {
    init: function () {
      initNote();
    },
  };
})();

jQuery(document).ready(function () {
  Note.init();
});
