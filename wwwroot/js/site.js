/**
 * SOVANNAPHUM RESTAURANT MANAGEMENT SYSTEM (RMS)
 * Interactive UI Engine & Client Helpers
 */

document.addEventListener('DOMContentLoaded', function () {
    // 1. Sidebar Toggle Logic (Desktop Collapse & Mobile Offcanvas)
    const sidebarToggleBtn = document.getElementById('rmsSidebarToggle');
    const backdrop = document.getElementById('rmsSidebarBackdrop');

    if (sidebarToggleBtn) {
        sidebarToggleBtn.addEventListener('click', function () {
            if (window.innerWidth >= 992) {
                document.body.classList.toggle('rms-sidebar-collapsed');
                localStorage.setItem('rms-sidebar-state', document.body.classList.contains('rms-sidebar-collapsed') ? 'collapsed' : 'expanded');
            } else {
                document.body.classList.toggle('rms-sidebar-mobile-open');
                if (backdrop) {
                    backdrop.classList.toggle('active', document.body.classList.contains('rms-sidebar-mobile-open'));
                }
            }
        });
    }

    if (backdrop) {
        backdrop.addEventListener('click', function () {
            document.body.classList.remove('rms-sidebar-mobile-open');
            backdrop.classList.remove('active');
        });
    }

    // Restore saved desktop collapse preference
    if (window.innerWidth >= 992 && localStorage.getItem('rms-sidebar-state') === 'collapsed') {
        document.body.classList.add('rms-sidebar-collapsed');
    }

    // 2. Phnom Penh Live Digital Clock (GMT+7)
    function updatePhnomPenhTime() {
        const timeDisplay = document.getElementById('rmsLiveClock');
        if (!timeDisplay) return;

        const options = {
            timeZone: 'Asia/Phnom_Penh',
            hour12: true,
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit'
        };
        const formatter = new Intl.DateTimeFormat('en-US', options);
        timeDisplay.textContent = formatter.format(new Date());
    }
    updatePhnomPenhTime();
    setInterval(updatePhnomPenhTime, 1000);

    // 3. Global Instant Search for Tables and Card Grids
    const globalSearchInput = document.getElementById('rmsGlobalSearch');
    if (globalSearchInput) {
        globalSearchInput.addEventListener('keyup', function (e) {
            const query = e.target.value.toLowerCase().trim();
            const tableRows = document.querySelectorAll('.rms-table tbody tr');
            const cards = document.querySelectorAll('.rms-searchable-card');

            if (tableRows.length > 0) {
                tableRows.forEach(row => {
                    const text = row.textContent.toLowerCase();
                    row.style.display = text.includes(query) ? '' : 'none';
                });
            }

            if (cards.length > 0) {
                cards.forEach(card => {
                    const text = card.textContent.toLowerCase();
                    card.closest('.col-xl-3, .col-lg-4, .col-md-6, .col-sm-12, .col-12, .col-lg-3').style.display = text.includes(query) ? '' : 'none';
                });
            }
        });
    }

    // 4. Highlight Active Navigation Item Based on URL Path
    const currentPath = window.location.pathname.toLowerCase();
    const navLinks = document.querySelectorAll('.rms-nav-link');

    navLinks.forEach(link => {
        const href = link.getAttribute('href');
        if (href) {
            const linkPath = href.toLowerCase();
            const isRoot = (linkPath === '/' || linkPath === '/home' || linkPath === '/home/index') &&
                           (currentPath === '/' || currentPath === '/home' || currentPath === '/home/index' || currentPath === '');
            const isDashboard = (linkPath.includes('/dashboard') && currentPath.includes('/dashboard'));
            const isMatch = linkPath !== '/' && !linkPath.startsWith('/#') && currentPath.startsWith(linkPath);

            if (isRoot || isDashboard || isMatch) {
                link.classList.add('active');
            }
        }
    });

    // 5. Delete Confirmation Modal Bridge
    window.confirmRmsDelete = function (deleteUrl, itemName) {
        const modal = document.getElementById('rmsDeleteModal');
        const modalItemName = document.getElementById('rmsDeleteModalItemName');
        const modalForm = document.getElementById('rmsDeleteModalForm');

        if (modal && modalForm) {
            if (modalItemName) {
                modalItemName.textContent = itemName || 'this record';
            }
            modalForm.setAttribute('action', deleteUrl);
            const bsModal = new bootstrap.Modal(modal);
            bsModal.show();
        } else {
            if (confirm(`Are you sure you want to delete "${itemName}"?`)) {
                window.location.href = deleteUrl;
            }
        }
    };
});
