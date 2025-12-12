document.addEventListener('DOMContentLoaded', function () {

    const form = document.querySelector('form');
    const emailInput = document.querySelector('input[name="Input.Email"]');
    const submitButton = document.querySelector('button[type="submit"]');

    addEmailIcon();

    if (emailInput) {
        emailInput.addEventListener('blur', validateEmail);
        emailInput.addEventListener('input', function () {
            clearError(emailInput);
            checkEmailFormat(emailInput);
        });

        emailInput.addEventListener('input', function () {
            this.value = this.value.toLowerCase().trim();
        });
    }

    if (form) {
        form.addEventListener('submit', handleFormSubmit);
    }

    addTipsSection();

    scrollToError();

    checkSuccessMessage();
});

function addEmailIcon() {
    const emailInput = document.querySelector('input[name="Input.Email"]');

    if (emailInput && !emailInput.previousElementSibling?.classList.contains('form-icon')) {
        const emailIcon = document.createElement('span');
        emailIcon.className = 'form-icon';
        emailIcon.innerHTML = '📧';
        emailInput.parentElement.insertBefore(emailIcon, emailInput);
    }
}

function validateEmail(e) {
    const input = e.target;
    const value = input.value.trim();
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!value) {
        showError(input, 'L\'adresse email est requise');
        return false;
    }

    if (!emailRegex.test(value)) {
        showError(input, 'Veuillez entrer une adresse email valide');
        return false;
    }

    clearError(input);
    input.classList.add('success');
    showSuccessCheckmark(input);
    return true;
}

function checkEmailFormat(input) {
    const value = input.value.trim();
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (value && emailRegex.test(value)) {
        input.classList.add('success');
        input.classList.remove('error');
        showSuccessCheckmark(input);
    } else {
        input.classList.remove('success');
        hideSuccessCheckmark(input);
    }
}

function showSuccessCheckmark(input) {
    let checkmark = input.parentElement.querySelector('.success-checkmark');

    if (!checkmark) {
        checkmark = document.createElement('span');
        checkmark.className = 'success-checkmark';
        checkmark.innerHTML = '✓';
        input.parentElement.appendChild(checkmark);
    }

    setTimeout(() => {
        checkmark.classList.add('show');
    }, 100);
}

function hideSuccessCheckmark(input) {
    const checkmark = input.parentElement.querySelector('.success-checkmark');
    if (checkmark) {
        checkmark.classList.remove('show');
    }
}

function showError(input, message) {
    input.classList.add('error');
    input.classList.remove('success');
    hideSuccessCheckmark(input);

    let errorSpan = input.parentElement.querySelector('.validation-error');
    if (!errorSpan) {
        errorSpan = document.createElement('span');
        errorSpan.className = 'validation-error';
        input.parentElement.appendChild(errorSpan);
    }

    errorSpan.textContent = message;
    errorSpan.style.display = 'block';
}

function clearError(input) {
    if (input.target) {
        input = input.target;
    }

    input.classList.remove('error');
    const errorSpan = input.parentElement.querySelector('.validation-error');
    if (errorSpan) {
        errorSpan.style.display = 'none';
    }
}

function handleFormSubmit(e) {
    const submitButton = document.querySelector('button[type="submit"]');
    const emailInput = document.querySelector('input[name="Input.Email"]');

    if (emailInput) {
        const emailEvent = {target: emailInput};
        if (!validateEmail(emailEvent)) {
            e.preventDefault();
            emailInput.focus();
            return false;
        }
    }

    if (submitButton) {
        submitButton.classList.add('loading');
        submitButton.disabled = true;

        if (emailInput) {
            sessionStorage.setItem('resendEmail', emailInput.value);
        }
    }
}

function addTipsSection() {
    const form = document.querySelector('form');
    if (!form) return;

    const tipsSection = document.createElement('div');
    tipsSection.className = 'tips-section';
    tipsSection.innerHTML = `
        <div class="tips-title">💡 Conseils utiles</div>
        <ul class="tips-list">
            <li>Vérifiez votre dossier spam ou courrier indésirable</li>
            <li>Assurez-vous d'utiliser la même adresse email que lors de l'inscription</li>
            <li>L'email peut prendre quelques minutes pour arriver</li>
            <li>Vérifiez que l'adresse email est correctement orthographiée</li>
        </ul>
    `;

    form.parentElement.appendChild(tipsSection);
}

function scrollToError() {
    const errorSummary = document.querySelector('[asp-validation-summary]');
    if (errorSummary) {
        const errorList = errorSummary.querySelector('ul');
        if (errorList && errorList.children.length > 0) {
            errorSummary.scrollIntoView({behavior: 'smooth', block: 'center'});
        }
    }
}

function checkSuccessMessage() {
    const urlParams = new URLSearchParams(window.location.search);
    const success = urlParams.get('success');

    if (success === 'true') {
        showSuccessAnimation();
    }
}

function showSuccessAnimation() {
    const wrapper = document.querySelector('.resend-wrapper');
    if (!wrapper) return;

    const email = sessionStorage.getItem('resendEmail');
    sessionStorage.removeItem('resendEmail');

    wrapper.innerHTML = `
        <div class="email-sent-animation">
            <div class="email-icon-large">✉️</div>
            <h2 class="resend-title">Email envoyé !</h2>
            <p class="resend-subtitle">
                Un email de confirmation a été envoyé à <strong>${email || 'votre adresse'}</strong>
            </p>
            <div class="alert-success-custom">
                <strong>✓ Succès !</strong> Veuillez vérifier votre boîte de réception et suivre les instructions dans l'email.
            </div>
            <div class="tips-section">
                <div class="tips-title">Prochaines étapes</div>
                <ul class="tips-list">
                    <li>Ouvrez l'email que nous venons d'envoyer</li>
                    <li>Cliquez sur le lien de confirmation</li>
                    <li>Vous pourrez ensuite vous connecter à votre compte</li>
                </ul>
            </div>
            <div class="resend-links" style="margin-top: 2rem;">
                <a href="/Identity/Account/Login" class="resend-link">
                    <span class="back-icon">←</span> Retour à la connexion
                </a>
            </div>
        </div>
    `;
}

document.addEventListener('focusin', function (e) {
    if (e.target.matches('.form-control-custom')) {
        e.target.parentElement.style.transform = 'scale(1.02)';
        e.target.parentElement.style.transition = 'transform 0.2s ease';
    }
});

document.addEventListener('focusout', function (e) {
    if (e.target.matches('.form-control-custom')) {
        e.target.parentElement.style.transform = 'scale(1)';
    }
});

let isSubmitting = false;
document.addEventListener('submit', function (e) {
    if (isSubmitting) {
        e.preventDefault();
        return false;
    }
    isSubmitting = true;
});


window.addEventListener('load', function () {
    const emailInput = document.querySelector('input[name="Input.Email"]');
    if (emailInput && !emailInput.value) {
        emailInput.focus();
    }
});