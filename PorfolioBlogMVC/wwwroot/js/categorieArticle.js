$(document).ready(function () {
    // Validation côté client pour les formulaires
    'use strict';
    var forms = document.querySelectorAll('.needs-validation');
    Array.prototype.slice.call(forms).forEach(function (form) {
        form.addEventListener('submit', function (event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            } else {
                // Afficher un indicateur de chargement
                var submitBtn = form.querySelector('button[type="submit"]');
                if (submitBtn) {
                    submitBtn.disabled = true;
                    submitBtn.innerHTML = '<i class="bi bi-hourglass-split me-2"></i>Traitement...';
                }
            }
            form.classList.add('was-validated');
        }, false);
    });

    // suppression
    $('form[action*="Delete"]').on('submit', function (e) {
        var confirmed = confirm('Êtes-vous vraiment sûr de vouloir supprimer cette catégorie ? Cette action est irréversible.');
        if (!confirmed) {
            e.preventDefault();
            return false;
        }
    });

    $('.categorie-card').each(function (index) {
        $(this).css('opacity', '0').delay(index * 100).animate({
            opacity: 1
        }, 600);
    });

    $('.categorie-card').hover(
        function () {
            $(this).find('.categorie-icon').css({
                'transform': 'scale(1.1)',
                'transition': 'transform 0.3s ease'
            });
        },
        function () {
            $(this).find('.categorie-icon').css({
                'transform': 'scale(1)',
                'transition': 'transform 0.3s ease'
            });
        }
    );

    // focus sur les champs de saisie
    $('.form-control').first().focus();

    $('form').on('submit', function () {
        $(this).find('button[type="submit"]').prop('disabled', true);
    });

    $(document).ajaxError(function (event, xhr, settings, thrownError) {
        console.error('Erreur AJAX:', thrownError);
        if (typeof showNotification === 'function') {
            showNotification('Une erreur s\'est produite. Veuillez réessayer.', 'error');
        }
    });

    window.showNotification = function (message, type) {
        alert(message);
    };

    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    $('.btn-group .btn').hover(
        function () {
            $(this).addClass('animate__animated animate__pulse');
        },
        function () {
            $(this).removeClass('animate__animated animate__pulse');
        }
    );

    $('.form-control[required]').on('blur', function () {
        if ($(this).val().trim() === '') {
            $(this).addClass('is-invalid');
        } else {
            $(this).removeClass('is-invalid').addClass('is-valid');
        }
    });

    // Reset de la validation lors de la saisie
    $('.form-control').on('input', function () {
        if ($(this).hasClass('is-invalid') && $(this).val().trim() !== '') {
            $(this).removeClass('is-invalid').addClass('is-valid');
        }
    });
});
