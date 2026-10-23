// ==========================================================================
// CodeLab — Apple Design System & Interactive Scripts
// ==========================================================================

document.addEventListener('DOMContentLoaded', () => {
    initTheme();
    initMobileSidebar();
    initProgramSearch();
    initCopyCodeButtons();
    initDashboardFilter();
});

// Theme preference is applied in theme.js before the first paint.
function applyTheme(theme) {
    document.documentElement.dataset.theme = theme;
    document.documentElement.dataset.bsTheme = theme;
    document.querySelectorAll('.theme-toggle-btn').forEach(button => {
        const label = `Switch to ${theme === 'dark' ? 'light' : 'dark'} theme`;
        button.setAttribute('aria-label', label);
        button.setAttribute('title', label);
    });
}

function initTheme() {
    let theme = document.documentElement.dataset.theme || 'light';
    let transition;
    let version = 0;
    applyTheme(theme);

    document.querySelectorAll('.theme-toggle-btn').forEach(button => {
        button.addEventListener('click', () => {
            theme = theme === 'dark' ? 'light' : 'dark';
            const nextTheme = theme;
            const currentVersion = ++version;
            const update = () => { if (currentVersion === version) applyTheme(nextTheme); };
            try { localStorage.setItem('codelab-theme', nextTheme); } catch { /* Theme still works without storage. */ }
            transition?.skipTransition();
            if (!document.startViewTransition || window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
                update();
                return;
            }
            const bounds = button.getBoundingClientRect();
            const x = bounds.left + bounds.width / 2;
            const y = bounds.top + bounds.height / 2;
            const radius = Math.hypot(Math.max(x, innerWidth - x), Math.max(y, innerHeight - y));
            transition = document.startViewTransition(update);
            transition.ready.then(() => {
                if (currentVersion !== version) return;
                document.documentElement.animate({
                    clipPath: [`circle(0px at ${x}px ${y}px)`, `circle(${radius}px at ${x}px ${y}px)`]
                }, {
                    duration: 550,
                    easing: 'cubic-bezier(.4, 0, 1, 1)',
                    pseudoElement: '::view-transition-new(root)'
                });
            }).catch(update);
        });
    });
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', event => {
        let saved;
        try { saved = localStorage.getItem('codelab-theme'); } catch { /* Use system preference. */ }
        if (saved !== 'light' && saved !== 'dark') {
            version++;
            transition?.skipTransition();
            theme = event.matches ? 'dark' : 'light';
            applyTheme(theme);
        }
    });
}

function initMobileSidebar() {
    const toggle = document.querySelector('.mobile-menu-toggle');
    const sidebar = document.querySelector('.app-sidebar');
    const backdrop = document.querySelector('.sidebar-backdrop');
    const header = document.querySelector('.site-header');
    const main = document.querySelector('.app-main');
    const close = document.querySelector('.sidebar-close');
    if (!toggle || !sidebar) return;

    function setOpen(open, restoreFocus = true) {
        sidebar.classList.toggle('show', open);
        if (backdrop) backdrop.hidden = !open;
        toggle.setAttribute('aria-expanded', String(open));
        document.body.style.overflow = open ? 'hidden' : '';
        if (header) header.inert = open;
        if (main) main.inert = open;
        if (open) close?.focus();
        else if (restoreFocus) toggle.focus();
    }
    toggle.addEventListener('click', () => setOpen(true));
    close?.addEventListener('click', () => setOpen(false));
    backdrop?.addEventListener('click', () => setOpen(false));
    document.addEventListener('keydown', event => {
        if (!sidebar.classList.contains('show')) return;
        if (event.key === 'Escape') setOpen(false);
        if (event.key === 'Tab') {
            const controls = [...sidebar.querySelectorAll('a, button, summary')].filter(control => control.getClientRects().length);
            const first = controls[0], last = controls[controls.length - 1];
            if (event.shiftKey && document.activeElement === first) {
                event.preventDefault(); last.focus();
            } else if (!event.shiftKey && document.activeElement === last) {
                event.preventDefault(); first.focus();
            }
        }
    });
    window.matchMedia('(min-width: 993px)').addEventListener('change', event => {
        if (event.matches && sidebar.classList.contains('show')) setOpen(false, false);
    });
}

function initProgramSearch() {
    const dialog = document.getElementById('program-search-dialog');
    const input = document.getElementById('globalProgramSearch');
    if (!dialog || !input) return;
    const results = [...dialog.querySelectorAll('.search-result')];
    const count = document.getElementById('search-result-count');
    const empty = document.getElementById('search-empty');
    let returnFocus;
    let previousOverflow;

    function filter() {
        const query = input.value.trim().toLowerCase();
        results.forEach(result => { result.hidden = !result.dataset.search.toLowerCase().includes(query); });
        const total = results.filter(result => !result.hidden).length;
        if (count) count.textContent = `${total} ${total === 1 ? 'program' : 'programs'}`;
        if (empty) empty.hidden = total !== 0;
    }
    function openSearch() {
        if (dialog.open || document.querySelector('.app-sidebar.show')) return;
        returnFocus = document.activeElement;
        previousOverflow = document.body.style.overflow;
        input.value = '';
        filter();
        dialog.showModal();
        document.body.style.overflow = 'hidden';
        input.focus();
    }
    document.querySelectorAll('.search-trigger').forEach(button => button.addEventListener('click', openSearch));
    input.addEventListener('input', filter);
    document.addEventListener('keydown', event => {
        if ((event.ctrlKey || event.metaKey) && event.key.toLowerCase() === 'k') {
            event.preventDefault();
            if (dialog.open) dialog.close(); else openSearch();
        }
    });
    dialog.addEventListener('close', () => {
        document.body.style.overflow = previousOverflow || '';
        returnFocus?.focus();
    });
    dialog.addEventListener('click', event => { if (event.target === dialog) dialog.close(); });
    dialog.addEventListener('keydown', event => {
        const visible = results.filter(result => !result.hidden);
        if (event.key === 'ArrowDown' || event.key === 'ArrowUp') {
            event.preventDefault();
            if (!visible.length) return;
            const index = visible.indexOf(document.activeElement);
            const next = event.key === 'ArrowDown' ? (index + 1) % visible.length : (index <= 0 ? visible.length - 1 : index - 1);
            visible[next].focus();
        }
        if (event.key === 'Enter' && document.activeElement === input) {
            event.preventDefault();
            visible[0]?.click();
        }
    });
}

// 3. macOS Code Snippet Copy Handler
function initCopyCodeButtons() {
    const copyBtns = document.querySelectorAll('.btn-copy-code');
    copyBtns.forEach(btn => {
        btn.addEventListener('click', () => {
            const targetId = btn.getAttribute('data-code-target');
            const codeEl = document.getElementById(targetId);
            if (!codeEl) return;

            const textToCopy = codeEl.innerText || codeEl.textContent;
            navigator.clipboard.writeText(textToCopy).then(() => {
                const originalHtml = btn.innerHTML;
                btn.innerHTML = '<i class="bi bi-check-lg text-success"></i> Copied!';
                setTimeout(() => {
                    btn.innerHTML = originalHtml;
                }, 2000);
            }).catch(err => {
                btn.textContent = 'Copy failed — try again';
                btn.setAttribute('aria-live', 'polite');
            });
        });
    });
}

// 4. Dashboard Search and Category Filter
function initDashboardFilter() {
    const searchInput = document.getElementById('programSearchInput');
    const filterPills = document.querySelectorAll('.filter-pill');
    const cards = document.querySelectorAll('.program-card-wrapper');

    if (!searchInput && filterPills.length === 0) return;

    const requestedCategory = new URLSearchParams(window.location.search).get('category');
    let activeFilter = [...filterPills].some(pill => pill.dataset.filter === requestedCategory) ? requestedCategory : 'all';

    function filterCards() {
        const query = (searchInput ? searchInput.value : '').toLowerCase().trim();

        let visibleCount = 0;
        cards.forEach(card => {
            const title = (card.getAttribute('data-title') || '').toLowerCase();
            const desc = (card.getAttribute('data-desc') || '').toLowerCase();
            const category = card.getAttribute('data-category') || '';

            const matchesCategory = activeFilter === 'all' || category === activeFilter;
            const matchesSearch = !query || title.includes(query) || desc.includes(query);

            if (matchesCategory && matchesSearch) {
                card.style.display = '';
                visibleCount++;
            } else {
                card.style.display = 'none';
            }
        });

        filterPills.forEach(pill => {
            const selected = pill.dataset.filter === activeFilter;
            pill.classList.toggle('active', selected);
            pill.setAttribute('aria-pressed', String(selected));
        });
        const count = document.getElementById('programCount');
        if (count) count.textContent = `${visibleCount} ${visibleCount === 1 ? 'program' : 'programs'}`;

        const noResults = document.getElementById('noResultsMessage');
        if (noResults) {
            noResults.style.display = visibleCount === 0 ? 'block' : 'none';
        }
    }

    if (searchInput) {
        searchInput.addEventListener('input', filterCards);
    }

    filterPills.forEach(pill => {
        pill.addEventListener('click', (e) => {
            e.preventDefault();
            filterPills.forEach(p => p.classList.remove('active'));
            pill.classList.add('active');
            activeFilter = pill.getAttribute('data-filter') || 'all';
            filterCards();
        });
    });
    filterCards();
}
