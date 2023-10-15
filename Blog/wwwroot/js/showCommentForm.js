document.getElementById("addComment").addEventListener("click", function () {
    var element = document.getElementById("commentForm");
    var button = document.getElementById("addComment");

    if (element.hasAttribute("hidden")) {
        element.removeAttribute("hidden");
        button.setAttribute("hidden", "true")
    }
});