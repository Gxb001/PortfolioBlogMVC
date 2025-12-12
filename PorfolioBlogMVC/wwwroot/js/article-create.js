document.addEventListener("DOMContentLoaded", () => {
    const input = document.getElementById('imageUrlInput');
    const preview = document.getElementById('imagePreview');
    const img = document.getElementById('previewImg');

    if (!input || !preview || !img) return;

    input.addEventListener('input', function () {
        const url = this.value;

        if (url && (url.startsWith('http://') || url.startsWith('https://'))) {
            img.src = url;

            img.onload = () => preview.style.display = 'block';
            img.onerror = () => preview.style.display = 'none';

        } else {
            preview.style.display = 'none';
        }
    });
});
