function openMenu() {
$(".dropdown-content").toggle();
}

window.onclick = function(event) {
  if (!event.target.matches('.dropbtn')) {
    $(".dropdown-content").css("display", "none");
  }
}