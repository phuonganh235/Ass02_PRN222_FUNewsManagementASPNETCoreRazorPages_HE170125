// FU News Management System - Site-wide JavaScript

// Configure Toastr
toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": true,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

// Loading spinner functions
function showLoading() {
    // Create loading overlay if it doesn't exist
    if (!document.getElementById('loadingOverlay')) {
        const overlay = document.createElement('div');
        overlay.id = 'loadingOverlay';
        overlay.style.cssText = `
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0, 0, 0, 0.5);
            display: flex;
            justify-content: center;
            align-items: center;
            z-index: 9999;
        `;
        overlay.innerHTML = '<div class="spinner-border text-light" style="width: 3rem; height: 3rem;" role="status"><span class="visually-hidden">Loading...</span></div>';
        document.body.appendChild(overlay);
    }
    document.getElementById('loadingOverlay').style.display = 'flex';
}

function hideLoading() {
    const overlay = document.getElementById('loadingOverlay');
    if (overlay) {
        overlay.style.display = 'none';
    }
}

// Confirm dialog wrapper
function confirmAction(message, callback) {
    if (confirm(message)) {
        callback();
    }
}

// Format date to local string
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-GB', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

// Escape HTML to prevent XSS
function escapeHtml(text) {
    const map = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#039;'
    };
    return text.replace(/[&<>"']/g, function (m) { return map[m]; });
}

// Handle AJAX errors
function handleAjaxError(error) {
    console.error('AJAX Error:', error);
    toastr.error('An error occurred. Please try again.');
    hideLoading();
}

// Auto-hide alerts after 5 seconds
document.addEventListener('DOMContentLoaded', function () {
    const alerts = document.querySelectorAll('.alert:not(.alert-permanent)');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });
});

// Prevent double form submission
document.addEventListener('DOMContentLoaded', function () {
    const forms = document.querySelectorAll('form');
    forms.forEach(function (form) {
        form.addEventListener('submit', function () {
            const submitBtn = form.querySelector('button[type="submit"]');
            if (submitBtn) {
                submitBtn.disabled = true;
                setTimeout(function () {
                    submitBtn.disabled = false;
                }, 3000);
            }
        });
    });
});

// Character counter for textareas with maxlength
document.addEventListener('DOMContentLoaded', function () {
    const textareas = document.querySelectorAll('textarea[maxlength]');
    textareas.forEach(function (textarea) {
        const maxLength = textarea.getAttribute('maxlength');
        const counter = document.createElement('small');
        counter.className = 'text-muted character-counter';
        counter.textContent = `0 / ${maxLength} characters`;
        textarea.parentNode.insertBefore(counter, textarea.nextSibling);

        textarea.addEventListener('input', function () {
            const currentLength = this.value.length;
            counter.textContent = `${currentLength} / ${maxLength} characters`;

            if (currentLength >= maxLength * 0.9) {
                counter.classList.add('text-danger');
                counter.classList.remove('text-muted');
            } else {
                counter.classList.add('text-muted');
                counter.classList.remove('text-danger');
            }
        });
    });
});

// Smooth scroll to top
function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
}

// Add scroll to top button
document.addEventListener('DOMContentLoaded', function () {
    const scrollBtn = document.createElement('button');
    scrollBtn.innerHTML = '<i class="fas fa-arrow-up"></i>';
    scrollBtn.className = 'btn btn-primary btn-scroll-top';
    scrollBtn.style.cssText = `
        position: fixed;
        bottom: 20px;
        right: 20px;
        display: none;
        z-index: 1000;
        border-radius: 50%;
        width: 50px;
        height: 50px;
    `;
    scrollBtn.onclick = scrollToTop;
    document.body.appendChild(scrollBtn);

    window.addEventListener('scroll', function () {
        if (window.scrollY > 300) {
            scrollBtn.style.display = 'block';
        } else {
            scrollBtn.style.display = 'none';
        }
    });
});

// Export functions for global use
window.showLoading = showLoading;
window.hideLoading = hideLoading;
window.confirmAction = confirmAction;
window.formatDate = formatDate;
window.escapeHtml = escapeHtml;
window.handleAjaxError = handleAjaxError;
window.scrollToTop = scrollToTop;