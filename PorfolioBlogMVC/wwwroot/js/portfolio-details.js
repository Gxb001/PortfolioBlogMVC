// portfolio-details.js

document.addEventListener('DOMContentLoaded', function () {
    initializeImageModal();
    initializeShareButtons();
});

// Modal pour afficher les images en grand
function initializeImageModal() {
    const modal = document.getElementById('imageModal');
    const modalImg = document.getElementById('modalImage');
    const closeBtn = document.querySelector('.close-modal');
    const galleryItems = document.querySelectorAll('.gallery-item');

    if (!modal || !modalImg) return;

    // Ouvrir le modal au clic sur une image
    galleryItems.forEach(item => {
        item.addEventListener('click', function () {
            const imageUrl = this.getAttribute('data-image');
            modal.classList.add('active');
            modalImg.src = imageUrl;
            document.body.style.overflow = 'hidden';
        });
    });

    // Fermer le modal
    const closeModal = () => {
        modal.classList.remove('active');
        document.body.style.overflow = 'auto';
    };

    if (closeBtn) {
        closeBtn.addEventListener('click', closeModal);
    }

    // Fermer au clic en dehors de l'image
    modal.addEventListener('click', function (e) {
        if (e.target === modal) {
            closeModal();
        }
    });

    // Fermer avec la touche Escape
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && modal.classList.contains('active')) {
            closeModal();
        }
    });
}

// Fonctions de partage
function initializeShareButtons() {
    const url = window.location.href;
    const title = document.querySelector('.project-title')?.textContent || 'Découvrez ce projet';

    // Stocker dans le contexte global pour les onclick
    window.shareData = {url, title};
}

function shareOnTwitter() {
    const {url, title} = window.shareData;
    const twitterUrl = `https://twitter.com/intent/tweet?text=${encodeURIComponent(title)}&url=${encodeURIComponent(url)}`;
    window.open(twitterUrl, '_blank', 'width=600,height=400');
}

function shareOnFacebook() {
    const {url} = window.shareData;
    const facebookUrl = `https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(url)}`;
    window.open(facebookUrl, '_blank', 'width=600,height=400');
}

function shareOnLinkedIn() {
    const {url, title} = window.shareData;
    const linkedInUrl = `https://www.linkedin.com/sharing/share-offsite/?url=${encodeURIComponent(url)}`;
    window.open(linkedInUrl, '_blank', 'width=600,height=400');
}

function copyLink() {
    const {url} = window.shareData;

    if (navigator.clipboard && navigator.clipboard.writeText) {
        navigator.clipboard.writeText(url).then(() => {
            showNotification('Lien copié dans le presse-papiers !', 'success');
        }).catch(err => {
            console.error('Erreur lors de la copie:', err);
            fallbackCopyLink(url);
        });
    } else {
        fallbackCopyLink(url);
    }
}

function fallbackCopyLink(url) {
    const textarea = document.createElement('textarea');
    textarea.value = url;
    textarea.style.position = 'fixed';
    textarea.style.opacity = '0';
    document.body.appendChild(textarea);
    textarea.select();

    try {
        document.execCommand('copy');
        showNotification('Lien copié dans le presse-papiers !', 'success');
    } catch (err) {
        showNotification('Impossible de copier le lien', 'error');
    }

    document.body.removeChild(textarea);
}

function showNotification(message, type = 'info') {
    // Supprimer les notifications existantes
    const existingNotifications = document.querySelectorAll('.share-notification');
    existingNotifications.forEach(n => n.remove());

    const notification = document.createElement('div');
    notification.className = `share-notification share-notification-${type}`;
    notification.innerHTML = `
        <div class="notification-content">
            <i class="fas ${type === 'success' ? 'fa-check-circle' : 'fa-exclamation-circle'}"></i>
            <span>${message}</span>
        </div>
    `;

    // Ajouter les styles
    const style = document.createElement('style');
    style.textContent = `
        .share-notification {
            position: fixed;
            top: 20px;
            right: 20px;
            z-index: 10000;
            background: white;
            padding: 1rem 1.5rem;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
            animation: slideInRight 0.3s ease, slideOutRight 0.3s ease 2.7s;
        }
        
        .share-notification-success {
            border-left: 4px solid #28a745;
        }
        
        .share-notification-error {
            border-left: 4px solid #dc3545;
        }
        
        .notification-content {
            display: flex;
            align-items: center;
            gap: 0.75rem;
            color: #333;
            font-weight: 600;
        }
        
        .share-notification-success i {
            color: #28a745;
        }
        
        .share-notification-error i {
            color: #dc3545;
        }
        
        @keyframes slideInRight {
            from {
                transform: translateX(100%);
                opacity: 0;
            }
            to {
                transform: translateX(0);
                opacity: 1;
            }
        }
        
        @keyframes slideOutRight {
            from {
                transform: translateX(0);
                opacity: 1;
            }
            to {
                transform: translateX(100%);
                opacity: 0;
            }
        }
    `;

    if (!document.querySelector('style[data-notification-styles]')) {
        style.setAttribute('data-notification-styles', 'true');
        document.head.appendChild(style);
    }

    document.body.appendChild(notification);

    // Supprimer après l'animation
    setTimeout(() => {
        notification.remove();
    }, 3000);
}

// Animation au scroll
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
};

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.style.animation = 'fadeInUp 0.6s ease forwards';
            observer.unobserve(entry.target);
        }
    });
}, observerOptions);

// Observer les sections
document.querySelectorAll('.project-description, .project-gallery, .sidebar-section').forEach(el => {
    el.style.opacity = '0';
    observer.observe(el);
});

// Animation CSS
const style = document.createElement('style');
style.textContent = `
    @keyframes fadeInUp {
        from {
            opacity: 0;
            transform: translateY(30px);
        }
        to {
            opacity: 1;
            transform: translateY(0);
        }
    }
`;
document.head.appendChild(style);