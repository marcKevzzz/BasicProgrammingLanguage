// ==========================================================================
// CodeLab — Apple Design System & Interactive Scripts
// ==========================================================================

document.addEventListener('DOMContentLoaded', () => {
    initArrayEditors();
    initGridPrograms();
    initMaze();
    initSortSimulation();
    initTheme();
    initSidebarGroups();
    initLessonDetails();
    initMobileSidebar();
    initProgramSearch();
    initCopyCodeButtons();
    initDashboardFilter();
    initToasts();
});

let toastTimer;
function showToast(message) {
    const toast = document.getElementById('app-toast');
    if (!toast) return;
    clearTimeout(toastTimer);
    toast.textContent = message;
    toast.hidden = false;
    toastTimer = setTimeout(() => { toast.hidden = true; }, 3500);
}

function initToasts() {
    const toast = document.getElementById('app-toast');
    if (toast?.textContent.trim()) showToast(toast.textContent.trim());
    document.addEventListener('click', event => {
        const button = event.target.closest('button[type="button"]');
        if (!button) return;
        const messages = { btnAddSubject: 'Subject added.', btnStart: 'Timer started.', btnPause: 'Timer paused.', btnReset: 'Timer reset.' };
        if (messages[button.id]) showToast(messages[button.id]);
        else if (button.classList.contains('password-visibility')) showToast(button.getAttribute('aria-pressed') === 'true' ? 'Password shown.' : 'Password hidden.');
    });
}

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

function initSidebarGroups() {
    const groups = [...document.querySelectorAll('.nav-group[data-category]')];
    let restored = false;
    groups.forEach(group => {
        const key = `codelab-nav-${group.dataset.category}`;
        try {
            group.open = !restored && localStorage.getItem(key) === 'true';
            restored ||= group.open;
        } catch { /* Native disclosures still work without storage. */ }
        group.addEventListener('toggle', () => {
            if (group.open) groups.forEach(other => { if (other !== group) other.open = false; });
            groups.forEach(item => {
                try { localStorage.setItem(`codelab-nav-${item.dataset.category}`, String(item.open)); }
                catch { /* Keep navigation usable without persistence. */ }
            });
        });
    });
}

function initLessonDetails() {
    document.querySelectorAll('.lesson-details').forEach((details, index) => {
        const key = `codelab-lesson-${window.location.pathname}-${index}`;
        try { details.open = localStorage.getItem(key) === 'true'; }
        catch { /* Disclosures still work without storage. */ }
        details.addEventListener('toggle', () => {
            try { localStorage.setItem(key, String(details.open)); }
            catch { /* Keep this page usable without persistence. */ }
        });
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
                showToast('Code copied.');
                const originalHtml = btn.innerHTML;
                btn.innerHTML = '<i class="bi bi-check-lg text-success"></i> Copied!';
                setTimeout(() => {
                    btn.innerHTML = originalHtml;
                }, 2000);
            }).catch(err => {
                showToast('Could not copy code.');
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

function initArrayEditors() {
    document.querySelectorAll('.array-editor').forEach(editor => {
        const matrix = editor.dataset.matrix === 'true';
        const grade = editor.dataset.grade === 'true';
        const form = editor.closest('form');
        const hidden = [editor.querySelector('[name="Input"]'), editor.querySelector('[name="SecondInput"]')];
        const fields = editor.querySelector('[data-editor-fields]');
        const parse = value => value.trim().split(/[;\n]+/).map(row => row.trim().split(/[,\s]+/).filter(Boolean));
        let values = hidden.map(input => parse(input.value || '0'));
        const input = (value, label) => {
            const field = document.createElement('input');
            field.type = 'number'; field.step = 'any'; field.required = true;
            field.min = grade ? '0' : '-1000000'; field.max = grade ? '100' : '1000000';
            field.className = 'form-apple-input'; field.value = value; field.setAttribute('aria-label', label);
            return field;
        };
        function addRow(value = '') {
            const row = document.createElement('div'); row.className = 'd-flex gap-2 mb-2';
            const number = input(value, grade ? 'Grade (0–100)' : 'Number');
            number.dataset.value = 'true'; row.append(number);
            const remove = document.createElement('button'); remove.type = 'button'; remove.className = 'btn-apple-secondary';
            remove.textContent = '×'; remove.setAttribute('aria-label', 'Remove row');
            remove.addEventListener('click', () => { if (fields.children.length > 1) row.remove(); });
            row.append(remove); fields.append(row);
        }
        function renderMatrix() {
            fields.replaceChildren();
            const rows = Number(editor.querySelector('[data-rows]').value);
            const cols = Number(editor.querySelector('[data-columns]').value);
            if (!Number.isInteger(rows) || !Number.isInteger(cols) || rows < 1 || rows > 10 || cols < 1 || cols > 10) return;
            const count = hidden[1].value ? 2 : 1;
            for (let m = 0; m < count; m++) {
                const group = document.createElement('div'); group.dataset.matrixIndex = m;
                const title = document.createElement('p'); title.className = 'form-apple-label'; title.textContent = count === 2 ? `Matrix ${m === 0 ? 'A' : 'B'}` : 'Matrix'; group.append(title);
                for (let r = 0; r < rows; r++) {
                    const row = document.createElement('div'); row.className = 'matrix-input-row';
                    for (let c = 0; c < cols; c++) row.append(input(values[m]?.[r]?.[c] ?? '0', `Matrix ${m + 1}, row ${r + 1}, column ${c + 1}`));
                    group.append(row);
                }
                fields.append(group);
            }
        }
        function capture() {
            if (matrix) values = [...fields.querySelectorAll('[data-matrix-index]')].map(group => [...group.querySelectorAll('.matrix-input-row')].map(row => [...row.querySelectorAll('input')].map(field => field.value)));
        }
        if (matrix) {
            editor.querySelector('[data-rows]').value = values[0].length;
            editor.querySelector('[data-columns]').value = values[0][0].length;
            editor.querySelectorAll('[data-rows], [data-columns]').forEach(control => control.addEventListener('change', () => { capture(); renderMatrix(); }));
            renderMatrix();
        } else {
            values[0].flat().forEach(value => addRow(value)); 
            editor.querySelector('[data-add-value]').addEventListener('click', () => { if (fields.children.length < 100) addRow(); });
        }
        form.addEventListener('submit', () => {
            if (matrix) { capture(); values.forEach((value, m) => { hidden[m].value = value.map(row => row.join(',')).join('\n'); }); }
            else {
                hidden[0].value = [...fields.querySelectorAll('[data-value]')].map(field => field.value).join(',');
            }
        });
    });
}

function initSortSimulation() {
    document.querySelectorAll('.sort-simulation').forEach(panel => {
        const original = panel.dataset.values.trim().split(/[,\s]+/).map(Number);
        const descending = panel.dataset.direction === 'descending';
        const cells = panel.querySelector('[data-sort-cells]');
        const message = panel.querySelector('[data-sort-message]');
        const next = panel.querySelector('[data-sort-next]');
        let values, pass, index;
        function render(active = -1) {
            cells.replaceChildren();
            values.forEach((value, position) => {
                const cell = document.createElement('span');
                cell.className = 'array-cell' + (active >= 0 && (position === active || position === active + 1) ? ' sort-comparing' : '');
                cell.textContent = value; cells.append(cell);
            });
        }
        function reset() { values = [...original]; pass = 0; index = 0; next.disabled = values.length < 2; message.textContent = values.length < 2 ? 'Already sorted.' : 'Select Next step to compare neighboring values.'; render(); }
        next.addEventListener('click', () => {
            const left = values[index], right = values[index + 1];
            const swap = descending ? left < right : left > right;
            if (swap) [values[index], values[index + 1]] = [right, left];
            message.textContent = `Pass ${pass + 1}: compare ${left} and ${right}. ${swap ? 'Swap them.' : 'Keep their order.'}`;
            render(index);
            index++;
            if (index >= values.length - 1 - pass) { pass++; index = 0; }
            if (pass >= values.length - 1) { next.disabled = true; message.textContent += ' Sorting complete.'; }
        });
        panel.querySelector('[data-sort-reset]').addEventListener('click', reset);
        reset();
    });
}

function getTicTacToeOutcome(board) {
    const lines = [];
    for (let i = 0; i < 3; i++) { lines.push(board[i]); lines.push(board.map(row => row[i])); }
    lines.push([board[0][0], board[1][1], board[2][2]], [board[0][2], board[1][1], board[2][0]]);
    const win = lines.find(line => line[0] && line.every(cell => cell === line[0]));
    return win ? win[0] : board.flat().every(Boolean) ? 'draw' : null;
}
function initGridPrograms() {
    document.querySelectorAll('[data-theater]').forEach(panel => {
        const seats = Array.from({ length: 5 }, () => Array(8).fill(false));
        const prices = [150, 150, 300, 300, 200].map(price => Array(8).fill(price));
        const grid = panel.querySelector('[data-seat-grid]');
        const status = panel.querySelector('[data-seat-status]');
        function render() {
            grid.replaceChildren();
            seats.forEach((row, r) => row.forEach((selected, c) => {
                const button = document.createElement('button'); button.type = 'button';
                const label = String.fromCharCode(65 + r) + (c + 1);
                button.className = 'seat-button'; button.textContent = label;
                button.setAttribute('aria-label', `Seat ${label}, ₱${prices[r][c]}`); button.setAttribute('aria-pressed', String(selected));
                button.addEventListener('click', () => { seats[r][c] = !seats[r][c]; render(); grid.children[r * 8 + c].focus(); });
                grid.append(button);
            }));
            const count = seats.flat().filter(Boolean).length;
            status.textContent = `${count} selected · ${40 - count} available`;
            const selection = []; let total = 0;
            seats.forEach((row, r) => row.forEach((selected, c) => { if (selected) { total += prices[r][c]; selection.push(String.fromCharCode(65 + r) + (c + 1)); } }));
            panel.querySelector('[data-seat-total]').textContent = `₱${total.toLocaleString('en-PH')}`;
            panel.querySelector('[data-seat-selection]').textContent = selection.length ? selection.join(', ') : 'No seats selected.';
        }
        panel.querySelector('[data-seat-reset]').addEventListener('click', () => { seats.forEach(row => row.fill(false)); render(); });
        render();
    });
    document.querySelectorAll('[data-tic-tac-toe]').forEach(panel => {
        let board, turn;
        const grid = panel.querySelector('[data-game-board]');
        const status = panel.querySelector('[data-game-status]');
        function render() {
            const outcome = getTicTacToeOutcome(board);
            status.textContent = outcome === 'draw' ? 'Draw. Start a new game.' : outcome ? `${outcome} wins!` : `${turn}’s turn`;
            grid.replaceChildren();
            board.forEach((row, r) => row.forEach((value, c) => {
                const button = document.createElement('button'); button.type = 'button'; button.className = 'tic-cell' + (getWinningCells(board).includes(r * 3 + c) ? ' tic-winning' : '');
                button.textContent = value; button.disabled = Boolean(value || outcome);
                button.setAttribute('aria-label', `Row ${r + 1}, column ${c + 1}: ${value || 'empty'}`);
                button.addEventListener('click', () => { board[r][c] = turn; turn = turn === 'X' ? 'O' : 'X'; render(); const outcome = getTicTacToeOutcome(board); if (outcome) { showToast(outcome === 'draw' ? 'Game ended in a draw.' : `${outcome} wins!`); if (outcome !== 'draw') celebrateWin(panel); } const available = [...grid.children].find(cell => !cell.disabled); available?.focus(); });
                grid.append(button);
            }));
        }
        function reset() { board = Array.from({ length: 3 }, () => Array(3).fill('')); turn = 'X'; render(); }
        panel.querySelector('[data-game-reset]').addEventListener('click', reset); reset();
    });
}

function getWinningCells(board) {
    const lines = [[0,1,2],[3,4,5],[6,7,8],[0,3,6],[1,4,7],[2,5,8],[0,4,8],[2,4,6]];
    const cells = board.flat();
    return lines.find(line => cells[line[0]] && line.every(i => cells[i] === cells[line[0]])) || [];
}
function celebrateWin(panel) {
    if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) return;
    const burst = document.createElement('div'); burst.className = 'confetti-burst'; burst.setAttribute('aria-hidden', 'true');
    for (let i = 0; i < 32; i++) {
        const piece = document.createElement('span');
        piece.style.setProperty('--x', `${Math.random() * 500 - 250}px`);
        piece.style.setProperty('--rotation', `${Math.random() * 720}deg`);
        piece.style.background = ['#22c55e','#f59e0b','#3b82f6','#ec4899'][i % 4]; burst.append(piece);
    }
    panel.append(burst); setTimeout(() => burst.remove(), 1600);
}
const mazeMap = ['S001000','1101010','0001000','0111110','0000000','1111101','000000E'].map(row => row.split(''));
function mazeDestination(map, row, col, direction) {
    const delta = { up: [-1,0], down: [1,0], left: [0,-1], right: [0,1] }[direction];
    if (!delta) return null;
    const r = row + delta[0], c = col + delta[1];
    return map[r]?.[c] && map[r][c] !== '1' ? [r,c] : null;
}
function initMaze() {
    document.querySelectorAll('[data-maze]').forEach(panel => {
        const grid = panel.querySelector('[data-maze-grid]'), status = panel.querySelector('[data-maze-status]');
        let row = 0, col = 0, moves = 0, won = false;
        function render(message = '') {
            grid.replaceChildren();
            mazeMap.forEach((line, r) => line.forEach((value, c) => {
                const cell = document.createElement('span');
                cell.className = 'maze-cell' + (value === '1' ? ' maze-wall' : '') + (r === row && c === col ? ' maze-player' : '');
                cell.textContent = value === 'S' || value === 'E' ? value : '';
                cell.setAttribute('aria-label', r === row && c === col ? 'Player' : value === '1' ? 'Wall' : value === 'E' ? 'Exit' : 'Path'); grid.append(cell);
            }));
            status.textContent = message || `${moves} moves. Find the exit.`;
        }
        function move(direction) {
            if (won) return;
            const target = mazeDestination(mazeMap, row, col, direction);
            if (!target) { status.textContent = 'Blocked. Try another direction.'; return; }
            [row,col] = target; moves++; won = mazeMap[row][col] === 'E';
            render(won ? `Exit reached in ${moves} moves!` : '');
            if (won) showToast('Maze complete!');
        }
        grid.addEventListener('keydown', event => {
            const direction = { ArrowUp:'up', ArrowDown:'down', ArrowLeft:'left', ArrowRight:'right' }[event.key];
            if (direction) { event.preventDefault(); move(direction); }
        });
        panel.querySelectorAll('[data-move]').forEach(button => button.addEventListener('click', () => move(button.dataset.move)));
        panel.querySelector('[data-maze-reset]').addEventListener('click', () => { row = 0; col = 0; moves = 0; won = false; render(); }); render();
    });
}
