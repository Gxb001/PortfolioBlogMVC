document.addEventListener('DOMContentLoaded', function () {
    initializeFilters();
    initializeAnimations();
});

// Gestion des filtres
function initializeFilters() {
    const filterButtons = document.querySelectorAll('.filter-btn');
    const portfolioCards = document.querySelectorAll('.portfolio-card');

    if (filterButtons.length === 0) return;

    filterButtons.forEach(button => {
        button.addEventListener('click', function () {
            const filter = this.getAttribute('data-filter');

            // Mise à jour des boutons actifs
            filterButtons.forEach(btn => btn.classList.remove('active'));
            this.classList.add('active');

            // Filtrage des cartes
            filterCards(portfolioCards, filter);
        });
    });
}

function filterCards(cards, filter) {
    cards.forEach(card => {
        // Animation de sortie
        card.style.transform = 'scale(0.8)';
        card.style.opacity = '0';

        setTimeout(() => {
            if (filter === 'all') {
                card.style.display = 'flex';
                // Animation d'entrée
                setTimeout(() => {
                    card.style.transform = 'scale(1)';
                    card.style.opacity = '1';
                }, 50);
            } else if (filter === 'mine' && card.getAttribute('data-owner') === 'mine') {
                card.style.display = 'flex';
                // Animation d'entrée
                setTimeout(() => {
                    card.style.transform = 'scale(1)';
                    card.style.opacity = '1';
                }, 50);
            } else {
                card.style.display = 'none';
            }
        }, 300);
    });
}

// Animations au scroll
function initializeAnimations() {
    const cards = document.querySelectorAll('.portfolio-card');

    if (cards.length === 0) return;

    // Observer pour détecter l'entrée dans le viewport
    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry, index) => {
            if (entry.isIntersecting) {
                // Ajouter un délai progressif pour chaque carte
                setTimeout(() => {
                    entry.target.style.animation = 'fadeInUp 0.6s ease forwards';
                }, index * 100);
                observer.unobserve(entry.target);
            }
        });
    }, {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    });

    // Initialiser les cartes avec opacité 0
    cards.forEach(card => {
        card.style.opacity = '0';
        observer.observe(card);
    });
}

// Animation CSS pour fadeInUp
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
    
    .portfolio-card {
        transition: all 0.3s ease;
    }
`;
document.head.appendChild(style);

// Confirmation de suppression
document.addEventListener('click', function (e) {
    if (e.target.closest('.btn-delete')) {
        if (!confirm('Êtes-vous sûr de vouloir supprimer ce projet ? Cette action est irréversible.')) {
            e.preventDefault();
        }
    }
});

function initializeSearch() {
    const searchInput = document.getElementById('portfolioSearch');
    if (!searchInput) return;

    const cards = document.querySelectorAll('.portfolio-card');

    searchInput.addEventListener('input', function () {
        const searchTerm = this.value.toLowerCase();

        cards.forEach(card => {
            const title = card.querySelector('.card-title').textContent.toLowerCase();
            const description = card.querySelector('.card-description').textContent.toLowerCase();

            if (title.includes(searchTerm) || description.includes(searchTerm)) {
                card.style.display = 'flex';
            } else {
                card.style.display = 'none';
            }
        });
    });
}