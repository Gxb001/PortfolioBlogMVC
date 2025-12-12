document.addEventListener('DOMContentLoaded', function () {


    const form = document.getElementById('registerForm');
    const emailInput = document.querySelector('input[name="Input.Email"]');
    const passwordInput = document.querySelector('input[name="Input.Password"]');
    const confirmPasswordInput = document.querySelector('input[name="Input.ConfirmPassword"]');
    const submitButton = document.getElementById('registerSubmit');

    //secu
    addInputIcons();

    addPasswordToggles();


    addPasswordStrengthIndicator();

    addPasswordRequirements();

    addPasswordMatchIndicator();

    if (emailInput) {
        emailInput.addEventListener('blur', validateEmail);
        emailInput.addEventListener('input', function () {
            clearError(emailInput);
            checkEmailFormat(emailInput);
        });
    }

    if (passwordInput) {
        passwordInput.addEventListener('input', function () {
            clearError(passwordInput);
            checkPasswordStrength(passwordInput.value);
            checkPasswordRequirements(passwordInput.value);
            if (confirmPasswordInput.value) {
                checkPasswordMatch();
            }
        });
        passwordInput.addEventListener('blur', validatePassword);
    }

    if (confirmPasswordInput) {
        confirmPasswordInput.addEventListener('input', function () {
            clearError(confirmPasswordInput);
            checkPasswordMatch();
        });
        confirmPasswordInput.addEventListener('blur', validateConfirmPassword);
    }
    if (form) {
        form.addEventListener('submit', handleFormSubmit);
    }

    animateExternalButtons();
});

function addInputIcons() {
    const emailInput = document.querySelector('input[name="Input.Email"]');
    const passwordInput = document.querySelector('input[name="Input.Password"]');
    const confirmPasswordInput = document.querySelector('input[name="Input.ConfirmPassword"]');

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

    if (confirmPasswordInput && !confirmPasswordInput.previousElementSibling?.classList.contains('form-icon')) {
        const confirmIcon = document.createElement('span');
        confirmIcon.className = 'form-icon';
        confirmIcon.innerHTML = '✓';
        confirmPasswordInput.parentElement.insertBefore(confirmIcon, confirmPasswordInput);
    }
}

function addPasswordToggles() {
    const passwordInputs = [
        document.querySelector('input[name="Input.Password"]'),
        document.querySelector('input[name="Input.ConfirmPassword"]')
    ];

    passwordInputs.forEach(input => {
        if (input && !input.parentElement.querySelector('.password-toggle')) {
            const toggleButton = document.createElement('button');
            toggleButton.type = 'button';
            toggleButton.className = 'password-toggle';
            toggleButton.innerHTML = '👁️';
            toggleButton.setAttribute('aria-label', 'Afficher/masquer le mot de passe');

            toggleButton.addEventListener('click', function () {
                const type = input.getAttribute('type') === 'password' ? 'text' : 'password';
                input.setAttribute('type', type);
                toggleButton.innerHTML = type === 'password' ? '👁️' : '👁️‍🗨️';
            });

            input.parentElement.appendChild(toggleButton);
        }
    });
}

function addPasswordStrengthIndicator() {
    const passwordInput = document.querySelector('input[name="Input.Password"]');
    if (!passwordInput) return;

    const strengthContainer = document.createElement('div');
    strengthContainer.className = 'password-strength';
    strengthContainer.innerHTML = `
        <div class="strength-bar">
            <div class="strength-bar-fill"></div>
        </div>
        <div class="strength-text"></div>
    `;

    passwordInput.parentElement.parentElement.appendChild(strengthContainer);
}

function checkPasswordStrength(password) {
    const strengthBar = document.querySelector('.strength-bar-fill');
    const strengthText = document.querySelector('.strength-text');

    if (!strengthBar || !strengthText) return;

    let strength = 0;

    // Length check
    if (password.length >= 8) strength++;
    if (password.length >= 12) strength++;

    // Character variety checks
    if (/[a-z]/.test(password)) strength++;
    if (/[A-Z]/.test(password)) strength++;
    if (/[0-9]/.test(password)) strength++;
    if (/[^a-zA-Z0-9]/.test(password)) strength++;

    // Update UI
    strengthBar.className = 'strength-bar-fill';
    strengthText.className = 'strength-text';

    if (strength === 0) {
        strengthBar.style.width = '0%';
        strengthText.textContent = '';
    } else if (strength <= 2) {
        strengthBar.classList.add('weak');
        strengthText.classList.add('weak');
        strengthText.textContent = 'Faible';
    } else if (strength <= 4) {
        strengthBar.classList.add('medium');
        strengthText.classList.add('medium');
        strengthText.textContent = 'Moyen';
    } else {
        strengthBar.classList.add('strong');
        strengthText.classList.add('strong');
        strengthText.textContent = 'Fort';
    }
}

function addPasswordRequirements() {
    const passwordInput = document.querySelector('input[name="Input.Password"]');
    if (!passwordInput) return;

    const requirementsContainer = document.createElement('div');
    requirementsContainer.className = 'password-requirements';
    requirementsContainer.innerHTML = `
        <div class="requirement" data-requirement="length">
            <span class="requirement-icon"></span>
            <span>Au moins 8 caractères</span>
        </div>
        <div class="requirement" data-requirement="lowercase">
            <span class="requirement-icon"></span>
            <span>Une lettre minuscule</span>
        </div>
        <div class="requirement" data-requirement="uppercase">
            <span class="requirement-icon"></span>
            <span>Une lettre majuscule</span>
        </div>
        <div class="requirement" data-requirement="number">
            <span class="requirement-icon"></span>
            <span>Un chiffre</span>
        </div>
    `;

    passwordInput.parentElement.parentElement.appendChild(requirementsContainer);
}

function checkPasswordRequirements(password) {
    const requirements = {
        length: password.length >= 8,
        lowercase: /[a-z]/.test(password),
        uppercase: /[A-Z]/.test(password),
        number: /[0-9]/.test(password)
    };

    Object.keys(requirements).forEach(key => {
        const element = document.querySelector(`[data-requirement="${key}"]`);
        if (element) {
            if (requirements[key]) {
                element.classList.add('met');
            } else {
                element.classList.remove('met');
            }
        }
    });
}

function addPasswordMatchIndicator() {
    const confirmPasswordInput = document.querySelector('input[name="Input.ConfirmPassword"]');
    if (!confirmPasswordInput) return;

    const matchIndicator = document.createElement('div');
    matchIndicator.className = 'password-match-indicator';
    matchIndicator.style.display = 'none';

    confirmPasswordInput.parentElement.parentElement.appendChild(matchIndicator);
}

function checkPasswordMatch() {
    const passwordInput = document.querySelector('input[name="Input.Password"]');
    const confirmPasswordInput = document.querySelector('input[name="Input.ConfirmPassword"]');
    const matchIndicator = document.querySelector('.password-match-indicator');

    if (!passwordInput || !confirmPasswordInput || !matchIndicator) return;

    const password = passwordInput.value;
    const confirmPassword = confirmPasswordInput.value;

    if (confirmPassword.length === 0) {
        matchIndicator.style.display = 'none';
        return;
    }

    matchIndicator.style.display = 'flex';

    if (password === confirmPassword) {
        matchIndicator.className = 'password-match-indicator match';
        matchIndicator.textContent = '✓ Les mots de passe correspondent';
        confirmPasswordInput.classList.add('success');
        confirmPasswordInput.classList.remove('error');
    } else {
        matchIndicator.className = 'password-match-indicator no-match';
        matchIndicator.textContent = '✗ Les mots de passe ne correspondent pas';
        confirmPasswordInput.classList.remove('success');
    }
}

function validateEmail(e) {
    const input = e.target;
    const value = input.value.trim();
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!value) {
        showError(input, 'L\'email est requis');
        return false;
    }

    if (!emailRegex.test(value)) {
        showError(input, 'Veuillez entrer une adresse email valide');
        return false;
    }

    clearError(input);
    input.classList.add('success');
    return true;
}

function checkEmailFormat(input) {
    const value = input.value.trim();
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (value && emailRegex.test(value)) {
        input.classList.add('success');
    } else {
        input.classList.remove('success');
    }
}

function validatePassword(e) {
    const input = e.target;
    const value = input.value;

    if (!value) {
        showError(input, 'Le mot de passe est requis');
        return false;
    }

    if (value.length < 8) {
        showError(input, 'Le mot de passe doit contenir au moins 8 caractères');
        return false;
    }

    if (!/[a-z]/.test(value)) {
        showError(input, 'Le mot de passe doit contenir au moins une lettre minuscule');
        return false;
    }

    if (!/[A-Z]/.test(value)) {
        showError(input, 'Le mot de passe doit contenir au moins une lettre majuscule');
        return false;
    }

    if (!/[0-9]/.test(value)) {
        showError(input, 'Le mot de passe doit contenir au moins un chiffre');
        return false;
    }

    clearError(input);
    return true;
}

function validateConfirmPassword(e) {
    const input = e.target;
    const passwordInput = document.querySelector('input[name="Input.Password"]');
    const value = input.value;

    if (!value) {
        showError(input, 'Veuillez confirmer votre mot de passe');
        return false;
    }

    if (value !== passwordInput.value) {
        showError(input, 'Les mots de passe ne correspondent pas');
        return false;
    }

    clearError(input);
    return true;
}

function showError(input, message) {
    input.classList.add('error');
    input.classList.remove('success');

    input.classList.add('shake');
    setTimeout(() => input.classList.remove('shake'), 300);

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
    const submitButton = document.getElementById('registerSubmit');
    const emailInput = document.querySelector('input[name="Input.Email"]');
    const passwordInput = document.querySelector('input[name="Input.Password"]');
    const confirmPasswordInput = document.querySelector('input[name="Input.ConfirmPassword"]');

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

    if (confirmPasswordInput) {
        const confirmEvent = {target: confirmPasswordInput};
        if (!validateConfirmPassword(confirmEvent)) {
            isValid = false;
        }
    }

    if (!isValid) {
        e.preventDefault();

        const firstError = document.querySelector('.form-control-custom.error');
        if (firstError) {
            firstError.scrollIntoView({behavior: 'smooth', block: 'center'});
            firstError.focus();
        }

        return false;
    }

    if (submitButton) {
        submitButton.classList.add('loading');
        submitButton.disabled = true;
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