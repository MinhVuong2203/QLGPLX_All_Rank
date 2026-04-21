export function initUI() {

    document.addEventListener("click", (e) => {

        // 🌙 THEME
        if (e.target.closest("#themeToggle")) {
            const html = document.documentElement;
            const current = html.getAttribute("data-theme") || "light";
            const next = current === "light" ? "dark" : "light";

            html.setAttribute("data-theme", next);
            localStorage.setItem("theme", next);
        }

        // 📚 SIDEBAR
        if (e.target.closest("#sidebarToggle")) {
            document.querySelector(".app-shell")
                ?.classList.toggle("sidebar-collapsed");
        }

        // 🔗 NAV CLICK (SPA)
        const nav = e.target.closest("[data-route]");
        if (nav) {
            const route = nav.getAttribute("data-route");
            if (route) {
                e.preventDefault();
                location.hash = "#/" + route;
            }
        }

    });
}

// highlight menu
export function setActiveNav(route) {
    document.querySelectorAll(".nav-item").forEach(el => {
        el.classList.remove("active");
    });

    const active = document.querySelector(`[data-route="${route}"]`);
    if (active) active.classList.add("active");
}

// load theme
export function initTheme() {
    const theme = localStorage.getItem("theme") || "light";
    document.documentElement.setAttribute("data-theme", theme);
}