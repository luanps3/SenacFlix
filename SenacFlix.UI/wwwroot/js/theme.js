(function () {
    const themeKey = 'senacflix_theme';
    const savedTheme = localStorage.getItem(themeKey);

    // Se já tiver tema salvo, aplica antes de carregar o resto
    if (savedTheme) {
        document.documentElement.setAttribute('data-bs-theme', savedTheme);
    } else {
        // Fallback para dark mode como padrão
        document.documentElement.setAttribute('data-bs-theme', 'dark');
        localStorage.setItem(themeKey, 'dark');
    }

    // Quando o DOM estiver pronto, vinculamos o botão
    document.addEventListener('DOMContentLoaded', function () {
        const btnToggle = document.getElementById('themeToggleBtn');
        const iconToggle = document.getElementById('themeToggleIcon');

        function updateIcon(theme) {
            if (!iconToggle) return;
            if (theme === 'dark') {
                iconToggle.className = 'fa fa-sun'; // Se tá dark, mostra botão para ir pro sol
            } else {
                iconToggle.className = 'fa fa-moon'; // Se tá light, mostra botão para ir pra lua
            }
        }

        const currentTheme = document.documentElement.getAttribute('data-bs-theme');
        updateIcon(currentTheme);

        if (btnToggle) {
            btnToggle.addEventListener('click', function (e) {
                e.preventDefault();
                let theme = document.documentElement.getAttribute('data-bs-theme');
                const newTheme = theme === 'dark' ? 'light' : 'dark';
                
                document.documentElement.setAttribute('data-bs-theme', newTheme);
                localStorage.setItem(themeKey, newTheme);
                updateIcon(newTheme);
            });
        }
    });
})();
