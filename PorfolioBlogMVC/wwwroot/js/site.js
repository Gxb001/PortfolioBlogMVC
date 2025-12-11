// Effet de réduction du header au scroll
window.addEventListener('scroll', function () {
    const header = document.querySelector('.site-header');
    if (window.scrollY > 50) {
        header.querySelector('.navbar').classList.add('scrolled');
    } else {
        header.querySelector('.navbar').classList.remove('scrolled');
    }
});

