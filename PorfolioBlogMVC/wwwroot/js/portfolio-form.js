document.addEventListener('DOMContentLoaded', function () {
    console.log('Portfolio form JS loaded');
    initializeImagePreview();
});

// Prévisualisation des images
function initializeImagePreview() {
    const imageUrlsTextarea = document.getElementById('imageUrls');
    if (!imageUrlsTextarea) return;

    const imagePreview = document.getElementById('imagePreview');
    const previewGrid = document.getElementById('previewGrid');

    imageUrlsTextarea.addEventListener('input', debounce(function () {
        const urls = this.value.split('\n').filter(url => url.trim() !== '');

        if (urls.length > 0) {
            imagePreview.style.display = 'block';
            previewGrid.innerHTML = '';

            urls.forEach((url, index) => {
                const cleanUrl = url.trim();
                if (cleanUrl) {
                    createPreviewItem(cleanUrl, index, previewGrid);
                }
            });
        } else {
            imagePreview.style.display = 'none';
        }
    }, 500));

    // Charger l'aperçu initial si des URLs existent déjà
    if (imageUrlsTextarea.value.trim()) {
        imageUrlsTextarea.dispatchEvent(new Event('input'));
    }
}

function createPreviewItem(url, index, container) {
    const previewItem = document.createElement('div');
    previewItem.className = 'preview-item';

    const img = document.createElement('img');
    img.src = url;
    img.alt = `Image ${index + 1}`;
    img.onerror = function () {
        this.src = 'data:image/svg+xml,%3Csvg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 200 200"%3E%3Crect fill="%23ddd" width="200" height="200"/%3E%3Ctext x="50%25" y="50%25" text-anchor="middle" dy=".3em" fill="%23999" font-size="16"%3EErreur%3C/text%3E%3C/svg%3E';
    };

    previewItem.appendChild(img);

    // Badge "Principale" pour la première image
    if (index === 0) {
        const badge = document.createElement('div');
        badge.className = 'preview-badge';
        badge.innerHTML = '<i class="fas fa-star"></i> Principale';
        previewItem.appendChild(badge);
    }

    // Bouton de suppression
    const removeBtn = document.createElement('button');
    removeBtn.type = 'button';
    removeBtn.className = 'preview-remove';
    removeBtn.innerHTML = '<i class="fas fa-times"></i>';
    removeBtn.onclick = function () {
        removeImageUrl(index);
    };
    previewItem.appendChild(removeBtn);

    container.appendChild(previewItem);
}

function removeImageUrl(index) {
    const imageUrlsTextarea = document.getElementById('imageUrls');
    const urls = imageUrlsTextarea.value.split('\n').filter(url => url.trim() !== '');

    urls.splice(index, 1);
    imageUrlsTextarea.value = urls.join('\n');
    imageUrlsTextarea.dispatchEvent(new Event('input'));
}

// Fonction utilitaire pour debounce
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func.apply(this, args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Validation du formulaire
document.addEventListener('submit', function (e) {
    const form = e.target;
    if (form.classList.contains('portfolio-form')) {
        const titre = form.querySelector('[name="Titre"]');
        const description = form.querySelector('[name="Description"]');

        let isValid = true;

        if (titre && titre.value.trim() === '') {
            alert('Le titre est requis');
            titre.focus();
            e.preventDefault();
            isValid = false;
        } else if (description && description.value.trim() === '') {
            alert('La description est requise');
            description.focus();
            e.preventDefault();
            isValid = false;
        }

        if (isValid) {
            // Afficher un indicateur de chargement
            const submitBtn = form.querySelector('button[type="submit"]');
            if (submitBtn) {
                submitBtn.disabled = true;
                submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Enregistrement...';
            }
        }
    }
});

// Animation des tags
document.querySelectorAll('.tag-checkbox').forEach(checkbox => {
    checkbox.addEventListener('change', function () {
        const label = this.querySelector('.tag-label');
        if (this.querySelector('input').checked) {
            label.style.animation = 'tagSelect 0.3s ease';
        }
    });
});

// Ajouter l'animation CSS
const style = document.createElement('style');
style.textContent = `
    @keyframes tagSelect {
        0% { transform: scale(1); }
        50% { transform: scale(1.1); }
        100% { transform: scale(1.05); }
    }
`;
document.head.appendChild(style);