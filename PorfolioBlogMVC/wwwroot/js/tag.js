document.addEventListener('DOMContentLoaded', function () {
    // Confirmation de suppression
    const confirmDeleteBtn = document.getElementById('confirmDelete');
    if (confirmDeleteBtn) {
        confirmDeleteBtn.addEventListener('click', function (e) {
            const isConfirmed = confirm('Êtes-vous vraiment sûr de vouloir supprimer ce tag ? Cette action est irréversible.');
            if (!isConfirmed) {
                e.preventDefault();
            }
        });
    }

    // Animation d'entrée pour les cartes
    const cards = document.querySelectorAll('.tag-card');
    cards.forEach((card, index) => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(20px)';
        setTimeout(() => {
            card.style.transition = 'opacity 0.5s ease-out, transform 0.5s ease-out';
            card.style.opacity = '1';
            card.style.transform = 'translateY(0)';
        }, index * 100);
    });

    // Validation côté client pour les formulaires
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            const inputs = form.querySelectorAll('input[required]');
            let isValid = true;
            inputs.forEach(input => {
                if (!input.value.trim()) {
                    input.classList.add('is-invalid');
                    isValid = false;
                } else {
                    input.classList.remove('is-invalid');
                }
            });
            if (!isValid) {
                e.preventDefault();
                alert('Veuillez remplir tous les champs requis.');
            }
        });
    });

    // Effet de focus sur les inputs
    const inputs = document.querySelectorAll('.form-control');
    inputs.forEach(input => {
        input.addEventListener('focus', function () {
            this.parentElement.classList.add('focused');
        });
        input.addEventListener('blur', function () {
            this.parentElement.classList.remove('focused');
        });
    });
});
