const assert = require('node:assert/strict');
const fs = require('node:fs');
const vm = require('node:vm');
const app = 'DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING';
const toast = { hidden: false, textContent: 'Saved.' };
let dismiss;
const timers = new Map();
let timerId = 0;
const listeners = {};
const context = vm.createContext({
    document: { addEventListener(name, handler) { listeners[name] = handler; }, getElementById: () => toast },
    clearTimeout(id) { timers.delete(id); },
    setTimeout(callback, delay) { assert.equal(delay, 3500); dismiss = callback; timers.set(++timerId, callback); return timerId; }
});
vm.runInContext(fs.readFileSync(`${app}/wwwroot/js/site.js`, 'utf8'), context);
context.initToasts();
assert.equal(toast.hidden, false);
dismiss();
assert.equal(toast.hidden, true);
context.showToast("Updated.");
assert.equal(toast.textContent, "Updated.");
assert.equal(timers.size, 1);
assert.equal(toast.hidden, false);
listeners.click({ target: { closest: () => ({ id: "btnAddSubject", disabled: false, classList: { contains: () => false } }) } });
assert.equal(toast.textContent, "Subject added.");
listeners.click({ target: { closest: () => ({ id: "btnStart", disabled: true }) } });
assert.equal(toast.textContent, "Timer started.");
context.document.getElementById = () => null;
context.initToasts();
const layout = fs.readFileSync(`${app}/Views/Shared/DELMUNDOMARCKEVIN_241639_Layout.cshtml`, 'utf8');
assert.match(layout, /class="app-toast"[^>]*role="status"/);
assert.doesNotMatch(layout.match(/<div class="app-toast"[^>]*>.*?<\/div>/s)[0], /<i\b|<svg\b|<button\b/);
console.log('Toast checks passed: auto-dismiss, no-message pages, accessible text-only markup.');
