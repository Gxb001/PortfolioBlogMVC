// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Effet de réduction du header au scroll
window.addEventListener('scroll', function () {
    const header = document.querySelector('.site-header');
    if (window.scrollY > 50) {
        header.querySelector('.navbar').classList.add('scrolled');
    } else {
        header.querySelector('.navbar').classList.remove('scrolled');
    }
});

