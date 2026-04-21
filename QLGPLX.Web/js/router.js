export function getRoute() {
    return location.hash.replace("#/", "") || "dashboard";
}

export async function loadPage(route) {
    const container = document.getElementById("pageContainer");
    loadCSS(route); // 🔥 thêm dòng này
    try {
        const res = await fetch(`pages/${route}.html`);

        if (!res.ok) throw new Error("404");

        container.innerHTML = await res.text();

    } catch (err) {
        console.error("Lỗi load page:", err);
        container.innerHTML = `<h3>Không tìm thấy trang: ${route}</h3>`;
    }
}

function loadCSS(route) {
    const id = "page-style";

    // xóa css cũ
    const old = document.getElementById(id);
    if (old) old.remove();

    // tạo css mới
    const link = document.createElement("link");
    link.id = id;
    link.rel = "stylesheet";
    link.href = `css/pages/${route}.css`;

    document.head.appendChild(link);
}