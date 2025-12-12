document.addEventListener('submit', function (e) {
    if (e.target.matches('form[action*="DeleteComment"]')) {
        if (!confirm('Êtes-vous sûr de vouloir supprimer ce commentaire ?')) {
            e.preventDefault();
        }
    }
});

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
