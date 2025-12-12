document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('form');
    const submitBtn = document.querySelector('.btn-reset');
    const emailInput = document.querySelector('input[type="email"]');

    form.addEventListener('submit', function (e) {
        submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Envoi en cours...';
        submitBtn.disabled = true;
    });

    emailInput.addEventListener('focus', function () {
        this.parentElement.classList.add('focused');
    });

    emailInput.addEventListener('blur', function () {
        this.parentElement.classList.remove('focused');
    });
});
