// Apply the preferred theme before styles load to avoid a flash on navigation.
(() => {
    let saved;
    try { saved = localStorage.getItem('codelab-theme'); } catch { /* Storage may be disabled. */ }
    const theme = saved === 'light' || saved === 'dark' ? saved :
        (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light');
    document.documentElement.dataset.theme = theme;
    document.documentElement.dataset.bsTheme = theme;
})();
