// Floating Background
jQuery(document).ready(function () {
    jQuery(".container-wrap").append(
        "<ul class='circles'><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li></ul>"
    );
});

AOS.init({
    duration: 1000,
    once: true,
    offset: 100
});

function closePopup() {

    const popup = document.getElementById("ticketPopup");

    if (!popup) return;

    popup.classList.add("popup-hide");

    setTimeout(() => {
        popup.remove();
    }, 400);

}

document.addEventListener("DOMContentLoaded", function () {

    const popup = document.getElementById("ticketPopup");

    if (popup) {
        setTimeout(closePopup, 4000);
    }

});