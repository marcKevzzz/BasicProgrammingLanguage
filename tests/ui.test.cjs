const assert = require('node:assert/strict');
const fs = require('node:fs');
const vm = require('node:vm');

function element(attributes = {}) {
    return {
        attributes, style: {}, handlers: {}, dataset: { filter: attributes['data-filter'] },
        value: '', textContent: '',
        getAttribute(name) { return this.attributes[name]; },
        setAttribute(name, value) { this.attributes[name] = value; },
        addEventListener(name, handler) { this.handlers[name] = handler; },
        classList: { toggle() {}, add() {}, remove() {} }
    };
}

function catalog(queryString, searchEnabled = true) {
    const search = element(), count = element(), empty = element();
    const pills = ['all', 'io', 'conditional'].map(value => element({ 'data-filter': value }));
    const cards = [
        element({ 'data-title': 'Age Calculator', 'data-desc': 'Convert years to days', 'data-category': 'io' }),
        element({ 'data-title': 'Password Checker', 'data-desc': 'Check character types', 'data-category': 'conditional' })
    ];
    const context = vm.createContext({
        URLSearchParams, window: { location: { search: queryString } },
        document: {
            addEventListener() {},
            getElementById(id) { return { programSearchInput: searchEnabled ? search : null, programCount: count, noResultsMessage: empty }[id]; },
            querySelectorAll(selector) { return selector === '.filter-pill' ? pills : cards; }
        }
    });
    vm.runInContext(fs.readFileSync('DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING/wwwroot/js/site.js', 'utf8'), context);
    context.initDashboardFilter();
    return { search, count, empty, pills, cards };
}

const view = catalog('?category=conditional');
assert.equal(view.count.textContent, '1 program');
assert.equal(view.cards[0].style.display, 'none');
assert.equal(view.pills[2].attributes['aria-pressed'], 'true');
view.search.value = ' PASSWORD ';
view.search.handlers.input();
assert.equal(view.count.textContent, '1 program');
view.search.value = 'no match';
view.search.handlers.input();
assert.equal(view.empty.style.display, 'block');
view.search.value = '';
view.pills[0].handlers.click({ preventDefault() {} });
assert.equal(view.count.textContent, '2 programs');
assert.equal(view.empty.style.display, 'none');
assert.equal(catalog('?category=unknown').count.textContent, '2 programs');
console.log('Catalog checks passed: category links, search, empty state, reset, accessible selection.');

// Theme behavior must survive blocked storage and browsers without view transitions.
function themeHarness({ reduced = false, transitions = false } = {}) {
    const button = element();
    button.getBoundingClientRect = () => ({ left: 900, top: 10, width: 40, height: 40 });
    const root = { dataset: { theme: 'light' }, animations: [], animate(frames, options) { this.animations.push({ frames, options }); } };
    let transitionCount = 0;
    const context = vm.createContext({
        innerWidth: 1000, innerHeight: 800, Math,
        localStorage: { getItem() { throw Error('Blocked'); }, setItem() { throw Error('Blocked'); } },
        window: { matchMedia: query => ({ matches: reduced && query.includes('reduced-motion'), addEventListener() {} }) },
        document: {
            documentElement: root, addEventListener() {}, querySelector() { return null; }, querySelectorAll() { return [button]; },
            startViewTransition: transitions ? update => {
                transitionCount++;
                update();
                return { ready: Promise.resolve(), skipTransition() {} };
            } : undefined
        }
    });
    vm.runInContext(fs.readFileSync('DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING/wwwroot/js/site.js', 'utf8'), context);
    context.initTheme();
    return { button, root, get transitionCount() { return transitionCount; } };
}
const fallback = themeHarness();
fallback.button.handlers.click();
assert.equal(fallback.root.dataset.theme, 'dark');
assert.equal(fallback.root.dataset.bsTheme, 'dark');
assert.equal(fallback.button.attributes['aria-label'], 'Switch to light theme');
fallback.button.handlers.click();
assert.equal(fallback.root.dataset.theme, 'light');
const reduced = themeHarness({ reduced: true, transitions: true });
reduced.button.handlers.click();
assert.equal(reduced.root.dataset.theme, 'dark');
assert.equal(reduced.transitionCount, 0);
const animated = themeHarness({ transitions: true });
animated.button.handlers.click();
animated.button.handlers.click();
assert.equal(animated.root.dataset.theme, 'light');
assert.equal(animated.transitionCount, 2);
console.log('Theme checks passed: blocked storage, fallback, reduced motion, rapid changes.');

// The global search uses the same catalog on every page and stays usable with no matches.
const searchInput = element(), trigger = element(), resultCount = element(), emptySearch = element();
const results = ['Age Calculator Input / Output', 'Countdown Timer Loops'].map(search => {
    const item = element(); item.dataset.search = search; item.hidden = false;
    item.focus = () => { searchDocument.activeElement = item; };
    item.click = () => { item.clicked = true; };
    return item;
});
searchInput.focus = () => { searchDocument.activeElement = searchInput; };
const dialog = element();
dialog.querySelectorAll = () => results;
dialog.showModal = () => { dialog.open = true; };
dialog.close = () => { dialog.open = false; dialog.handlers.close(); };
const searchDocument = {
    activeElement: { focus() {} }, body: { style: {} }, handlers: {},
    addEventListener(name, handler) { this.handlers[name] = handler; },
    querySelector() { return null; }, querySelectorAll() { return [trigger]; },
    getElementById(id) { return { 'program-search-dialog': dialog, globalProgramSearch: searchInput, 'search-result-count': resultCount, 'search-empty': emptySearch }[id]; }
};
const searchContext = vm.createContext({ document: searchDocument });
vm.runInContext(fs.readFileSync('DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING/wwwroot/js/site.js', 'utf8'), searchContext);
searchContext.initProgramSearch();
trigger.handlers.click();
assert.equal(dialog.open, true);
assert.equal(resultCount.textContent, '2 programs');
searchInput.value = ' COUNTDOWN ';
searchInput.handlers.input();
assert.equal(resultCount.textContent, '1 program');
assert.equal(results[0].hidden, true);
let prevented = false;
dialog.handlers.keydown({ key: 'Enter', preventDefault() { prevented = true; } });
assert.equal(prevented, true);
assert.equal(results[1].clicked, true);
searchInput.value = '<script>';
searchInput.handlers.input();
assert.equal(resultCount.textContent, '0 programs');
assert.equal(emptySearch.hidden, false);
dialog.close();
assert.equal(searchDocument.body.style.overflow, '');
console.log('Global search checks passed: normalization, keyboard open, empty state, scroll restoration.');


// The reveal uses viewport coordinates and the root snapshot, not the content panel.
Promise.resolve().then(() => {
    assert.equal(animated.root.animations.length, 1);
    const { frames, options } = animated.root.animations[0];
    assert.equal(options.pseudoElement, '::view-transition-new(root)');
    assert.equal(frames.clipPath[0], 'circle(0px at 920px 30px)');
    const radius = Number(frames.clipPath[1].match(/circle\(([^p]+)px/)[1]);
    for (const [x, y] of [[0, 0], [1000, 0], [0, 800], [1000, 800]]) {
        assert.ok(radius >= Math.hypot(x - 920, y - 30), 'Reveal must cover every viewport corner');
    }
    console.log('Full-screen reveal checks passed: viewport origin, root snapshot, and all four corners.');
});

const categoryOnly = catalog('', false);
categoryOnly.pills[2].handlers.click({ preventDefault() {} });
assert.equal(categoryOnly.count.textContent, '1 program');
assert.equal(categoryOnly.cards[0].style.display, 'none');
console.log('Category filtering works without a duplicate page search.');

function sidebarGroupsHarness(saved = {}, blocked = false) {
    const groups = ['io', 'conditional'].map(category => {
        const group = element();
        group.dataset.category = category;
        group.open = false;
        return group;
    });
    const context = vm.createContext({
        localStorage: {
            getItem(key) { if (blocked) throw Error('Storage blocked'); return saved[key] ?? null; },
            setItem(key, value) { if (blocked) throw Error('Storage blocked'); saved[key] = value; }
        },
        document: { addEventListener() {}, querySelectorAll() { return groups; } }
    });
    vm.runInContext(fs.readFileSync('DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING/wwwroot/js/site.js', 'utf8'), context);
    context.initSidebarGroups();
    return groups;
}
const savedGroups = {};
const firstVisit = sidebarGroupsHarness(savedGroups);
assert.equal(firstVisit[0].open, false);
firstVisit[0].open = true;
firstVisit[0].handlers.toggle();
const nextPage = sidebarGroupsHarness(savedGroups);
assert.equal(nextPage[0].open, true);
assert.equal(nextPage[1].open, false);
nextPage[1].open = true;
nextPage[1].handlers.toggle();
assert.equal(nextPage[0].open, false);
assert.equal(sidebarGroupsHarness(savedGroups)[1].open, true);
nextPage[1].open = false;
nextPage[1].handlers.toggle();
assert.equal(sidebarGroupsHarness(savedGroups)[0].open, false);
const privateGroups = sidebarGroupsHarness({}, true);
privateGroups[0].open = true;
assert.doesNotThrow(() => privateGroups[0].handlers.toggle());
console.log('Sidebar state checks passed: closed defaults, navigation/refresh restoration, closing, blocked storage.');

const passwordSource = fs.readFileSync('DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING/Views/DELMUNDOMARCKEVIN_241639_Conditional/DELMUNDOMARCKEVIN_241639_PasswordStrength.cshtml', 'utf8');
const passwordScript = passwordSource.match(/@section Scripts[\s\S]*?<script>([\s\S]*?)<\/script>/)[1];
const visibilityButton = element(), passwordInput = { type: 'password' }, eye = { className: 'bi bi-eye' };
vm.runInNewContext(passwordScript, {
    document: { getElementById(id) { return { btnTogglePasswordVisibility: visibilityButton, password: passwordInput, eyeIcon: eye }[id]; } }
});
visibilityButton.handlers.click();
assert.equal(passwordInput.type, 'text');
assert.equal(visibilityButton.attributes['aria-label'], 'Hide password');
assert.equal(visibilityButton.attributes['aria-pressed'], 'true');
visibilityButton.handlers.click();
assert.equal(passwordInput.type, 'password');
assert.equal(visibilityButton.attributes['aria-label'], 'Show password');
assert.equal(visibilityButton.attributes['aria-pressed'], 'false');
console.log('Password visibility checks passed: toggling and accessible button state.');

function lessonHarness(pathname, saved = {}, blocked = false) {
    const details = [element(), element()];
    const context = vm.createContext({
        window: { location: { pathname } },
        localStorage: {
            getItem(key) { if (blocked) throw Error('Blocked'); return saved[key] ?? null; },
            setItem(key, value) { if (blocked) throw Error('Blocked'); saved[key] = value; }
        },
        document: { addEventListener() {}, querySelectorAll() { return details; } }
    });
    vm.runInContext(fs.readFileSync('DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING/wwwroot/js/site.js', 'utf8'), context);
    context.initLessonDetails();
    return details;
}
const lessonsSaved = {};
const lessons = lessonHarness('/DELMUNDOMARCKEVIN_241639_InputOutput/AgeCalculator', lessonsSaved);
assert.equal(lessons[0].open, false);
lessons.forEach(item => { item.open = true; item.handlers.toggle(); });
assert.ok(lessonHarness('/DELMUNDOMARCKEVIN_241639_InputOutput/AgeCalculator', lessonsSaved).every(item => item.open));
assert.ok(lessonHarness('/DELMUNDOMARCKEVIN_241639_Conditional/BmiChecker', lessonsSaved).every(item => !item.open));
lessons[0].open = false;
lessons[0].handlers.toggle();
assert.equal(lessonHarness('/DELMUNDOMARCKEVIN_241639_InputOutput/AgeCalculator', lessonsSaved)[0].open, false);
assert.doesNotThrow(() => lessonHarness('/test', {}, true)[0].handlers.toggle());
console.log('Lesson disclosures passed: per-program refresh restoration, independent states, closed defaults, blocked storage.');

const timerSource = fs.readFileSync('DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING/Views/DELMUNDOMARCKEVIN_241639_Repetition/DELMUNDOMARCKEVIN_241639_CountdownTimer.cshtml', 'utf8');
const timerScript = timerSource.match(/@section Scripts[\s\S]*?<script>([\s\S]*?)<\/script>/)[1].replace('@duration', '2');
const timerElements = Object.fromEntries(['timerNumberDisplay', 'timerProgressRing', 'timerStatusBadge', 'btnStart', 'btnPause', 'btnReset'].map(id => [id, element()]));
let tick, tones = 0;
class TestAudio {
    state = 'running'; currentTime = 0; destination = {};
    resume() { return Promise.resolve(); }
    createOscillator() { return { frequency: {}, connect() {}, start() { tones++; }, stop() {} }; }
    createGain() { return { gain: { setValueAtTime() {}, linearRampToValueAtTime() {}, exponentialRampToValueAtTime() {} }, connect() {} }; }
}
vm.runInNewContext(timerScript, {
    window: { AudioContext: TestAudio },
    document: { getElementById(id) { return timerElements[id]; } },
    setInterval(callback) { tick = callback; return 1; }, clearInterval() {}
});
timerElements.btnStart.handlers.click();
tick();
assert.equal(tones, 0);
tick();
assert.equal(tones, 3);
assert.equal(timerElements.timerStatusBadge.textContent, 'Finished!');
timerElements.btnReset.handlers.click();
assert.equal(timerElements.timerNumberDisplay.textContent, 2);
console.log('Countdown checks passed: completion alarm, visible status, reset.');

for (const direction of ['ascending', 'descending']) {
    const next = element(), reset = element(), message = element(), play = element(), pause = element();
    const cells = { children: [], replaceChildren() { this.children = []; }, append(cell) { this.children.push(cell); }, insertBefore(b, a) { this.children.splice(this.children.indexOf(b), 1); this.children.splice(this.children.indexOf(a), 0, b); } };
    const panel = { dataset: { values: '3,1,2', direction }, querySelector(selector) { return { '[data-sort-play]': play, '[data-sort-pause]': pause, '[data-sort-next]': next, '[data-sort-reset]': reset, '[data-sort-message]': message, '[data-sort-cells]': cells }[selector]; } };
    let pending;
    const context = vm.createContext({ setTimeout(fn) { pending = fn; return 1; }, clearTimeout() { pending = null; }, window: { matchMedia() { return { matches: true }; } }, document: { addEventListener() {}, querySelectorAll() { return [panel]; }, createElement() { return element(); } } });
    vm.runInContext(fs.readFileSync('DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING/wwwroot/js/site.js', 'utf8'), context);
    context.initSortSimulation();
    assert.equal(play.disabled, true);
    assert.equal(pause.disabled, false);
    pending();
    pause.handlers.click();
    assert.equal(pending, null);
    assert.equal(play.disabled, false);
    for (let i = 0; i < 3; i++) next.handlers.click();
    assert.equal(cells.children.map(cell => cell.textContent).join(','), direction === 'ascending' ? '1,2,3' : '3,2,1');
    assert.equal(next.disabled, true);
    reset.handlers.click();
    assert.equal(cells.children.map(cell => cell.textContent).join(','), '3,1,2');
}
console.log('Sort simulation passed: both directions, completion, restart.');
