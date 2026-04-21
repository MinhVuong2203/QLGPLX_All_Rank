import { getRoute, loadPage } from "./router.js";
import { initUI, setActiveNav, initTheme } from "./ui.js";

async function runPage(route) {

    if (route === "ds_congdan") {
        const module = await import("./pages/ds_congdan.js");
        module.init();
    }

    if (route === "dashboard") {
        console.log("Dashboard");
    }
}

async function app() {

    initTheme();
    initUI();

    const route = getRoute();

    await loadPage(route);
    setActiveNav(route);

    runPage(route);
}

// route change
window.addEventListener("DOMContentLoaded", app);
window.addEventListener("hashchange", app);