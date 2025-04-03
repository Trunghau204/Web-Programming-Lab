// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $.get("/ShoppingCart/GetCartCount", function (data) {
        $("#cart-count").text(data);
    });
});
