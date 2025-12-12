document.addEventListener('DOMContentLoaded', function () {
    const createModal = document.getElementById('createModal');

    if (createModal) {
        createModal.addEventListener('show.bs.modal', function () {
            loadCreateForm();
        });

        createModal.addEventListener('hidden.bs.modal', function () {
            const modalBody = document.getElementById('createModalBody');
            if (modalBody) {
                modalBody.innerHTML = '<div class="text-center"><div class="spinner-border" role="status"><span class="visually-hidden">Chargement...</span></div></div>';
            }
        });
    }
});

// Fonction pour charger le formulaire de création via AJAX
function loadCreateForm() {
    const modalBody = document.getElementById('createModalBody');

    if (!modalBody) return;

    // Afficher un spinner pendant le chargement
    modalBody.innerHTML = `
        <div class="text-center">
            <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Chargement...</span>
            </div>
            <p class="mt-2">Chargement du formulaire...</p>
        </div>
    `;

    // Charger le formulaire via AJAX
    fetch('/Article/Create', {
        method: 'GET',
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Erreur lors du chargement du formulaire');
            }
            return response.text();
        })
        .then(html => {
            modalBody.innerHTML = html;

            if (typeof jQuery !== 'undefined' && jQuery.validator) {
                const form = modalBody.querySelector('form');
                if (form) {
                    jQuery(form).removeData('validator');
                    jQuery(form).removeData('unobtrusiveValidation');
                    jQuery.validator.unobtrusive.parse(form);
                }
            }
        })
        .catch(error => {
            console.error('Erreur:', error);
            modalBody.innerHTML = `
            <div class="alert alert-danger" role="alert">
                <i class="bi bi-exclamation-triangle"></i>
                Une erreur est survenue lors du chargement du formulaire.
                Veuillez réessayer ou <a href="/Article/Create" class="alert-link">cliquer ici</a> pour accéder à la page complète.
            </div>
        `;
        });
}

function showNotification(message, type = 'info') {
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type} alert-dismissible fade show position-fixed top-0 start-50 translate-middle-x mt-3`;
    alertDiv.style.zIndex = '9999';
    alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;

    document.body.appendChild(alertDiv);

    setTimeout(() => {
        alertDiv.remove();
    }, 5000);
}

document.addEventListener('submit', function (e) {
    if (e.target.matches('form[action*="DeleteComment"]')) {
        if (!confirm('Êtes-vous sûr de vouloir supprimer ce commentaire ?')) {
            e.preventDefault();
        }
    }
});