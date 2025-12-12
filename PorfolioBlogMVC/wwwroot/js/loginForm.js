document.addEventListener('DOMContentLoaded', function () {

    const form = document.getElementById('account');
    const emailInput = document.querySelector('input[name="Input.Email"]');
    const passwordInput = document.querySelector('input[name="Input.Password"]');
    const submitButton = document.getElementById('login-submit');

    addInputIcons();

    addPasswordToggle();

    if (emailInput) {
        emailInput.addEventListener('blur', validateEmail);
        emailInput.addEventListener('input', clearError);
    }

    if (passwordInput) {
        passwordInput.addEventListener('blur', validatePassword);
        passwordInput.addEventListener('input', clearError);
    }

    if (form) {
        form.addEventListener('submit', handleFormSubmit);
    }

    scrollToError();

    animateExternalButtons();
});

function addInputIcons() {
    const emailInput = document.querySelector('input[name="Input.Email"]');
    const passwordInput = document.querySelector('input[name="Input.Password"]');

    if (emailInput && !emailInput.previousElementSibling?.classList.contains('form-icon')) {
        const emailIcon = document.createElement('span');
        emailIcon.className = 'form-icon';
        emailIcon.innerHTML = '📧';
        emailInput.parentElement.insertBefore(emailIcon, emailInput);
    }

    if (passwordInput && !passwordInput.previousElementSibling?.classList.contains('form-icon')) {
        const passwordIcon = document.createElement('span');
        passwordIcon.className = 'form-icon';
        passwordIcon.innerHTML = '🔒';
        passwordInput.parentElement.insertBefore(passwordIcon, passwordInput);
    }
}

function addPasswordToggle() {
    const passwordInput = document.querySelector('input[name="Input.Password"]');

    if (passwordInput && !passwordInput.parentElement.querySelector('.password-toggle')) {
        const toggleButton = document.createElement('button');
        toggleButton.type = 'button';
        toggleButton.className = 'password-toggle';
        toggleButton.innerHTML = '👁️';
        toggleButton.setAttribute('aria-label', 'Toggle password visibility');

        toggleButton.addEventListener('click', function () {
            const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordInput.setAttribute('type', type);
            toggleButton.innerHTML = type === 'password' ? '👁️' : '👁️‍🗨️';
        });

        passwordInput.parentElement.appendChild(toggleButton);
    }
}

function validateEmail(e) {
    const input = e.target;
    const value = input.value.trim();
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!value) {
        showError(input, 'Email is required');
        return false;
    }

    if (!emailRegex.test(value)) {
        showError(input, 'Please enter a valid email address');
        return false;
    }

    clearError(input);
    return true;
}

function validatePassword(e) {
    const input = e.target;
    const value = input.value;

    if (!value) {
        showError(input, 'Password is required');
        return false;
    }

    if (value.length < 6) {
        showError(input, 'Password must be at least 6 characters');
        return false;
    }

    clearError(input);
    return true;
}

function showError(input, message) {
    input.classList.add('error');

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
    const submitButton = document.getElementById('login-submit');
    const emailInput = document.querySelector('input[name="Input.Email"]');
    const passwordInput = document.querySelector('input[name="Input.Password"]');

    let isValid = true;

    if (emailInput) {
        const emailEvent = {target: emailInput};
        if (!validateEmail(emailEvent)) {
            isValid = false;
        }
    }

    if (passwordInput) {
        const passwordEvent = {target: passwordInput};
        if (!validatePassword(passwordEvent)) {
            isValid = false;
        }
    }

    if (!isValid) {
        e.preventDefault();
        return false;
    }

    if (submitButton) {
        submitButton.classList.add('loading');
        submitButton.disabled = true;
    }
}

function scrollToError() {
    const errorSummary = document.querySelector('[asp-validation-summary]');
    if (errorSummary && errorSummary.querySelector('li')) {
        errorSummary.scrollIntoView({behavior: 'smooth', block: 'center'});
    }
}

function animateExternalButtons() {
    const externalButtons = document.querySelectorAll('.btn-external');
    externalButtons.forEach((button, index) => {
        button.style.opacity = '0';
        button.style.transform = 'translateY(20px)';

        setTimeout(() => {
            button.style.transition = 'all 0.5s ease';
            button.style.opacity = '1';
            button.style.transform = 'translateY(0)';
        }, 100 * index);
    });
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

const rememberCheckbox = document.querySelector('input[type="checkbox"]');
if (rememberCheckbox) {
    rememberCheckbox.addEventListener('change', function () {
        if (this.checked) {
            this.parentElement.style.transform = 'scale(1.05)';
            setTimeout(() => {
                this.parentElement.style.transform = 'scale(1)';
            }, 200);
        }
    });
}