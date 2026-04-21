import { get, del } from "../api.js";

export async function init() {
    await loadData();
}

// =======================
// LOAD DATA
// =======================
async function loadData() {
    try {
        const data = await get("/CongDan");
        renderTable(data);
    } catch (err) {
        console.error("Lỗi load dữ liệu:", err);
    }
}

// =======================
// RENDER
// =======================
function renderTable(data) {
    const tbody = document.getElementById("congdanTableBody");
    const noData = document.getElementById("congdanNoDataRow");
    const total = document.getElementById("congdanTotalBadge");

    if (!tbody) return;

    tbody.innerHTML = "";

    if (!data || data.length === 0) {
        noData.style.display = "";
        total.innerText = "Tổng: 0";
        return;
    }

    noData.style.display = "none";

    data.forEach(cd => {
        const tr = document.createElement("tr");

        tr.innerHTML = `
            <td>
                <img src="${cd.anh3x4 || '/images/default.png'}"
                     width="48" height="64"
                     style="object-fit:cover;border-radius:6px;">
            </td>
            <td>${cd.hoTen}</td>
            <td>${formatDate(cd.ngaySinh)}</td>
            <td>${cd.tuoi}</td>
            <td>${cd.gioiTinh || ""}</td>
            <td>${cd.soDienThoai || ""}</td>
            <td>${cd.email || ""}</td>
            <td>${cd.diaChi || ""}</td>
            <td>${cd.tinhTrangSucKhoe || ""}</td>
            <td>${formatDate(cd.ngayKhamSucKhoe)}</td>
            <td>
                ${cd.giayKhamSucKhoe
                ? `<a href="${cd.giayKhamSucKhoe}" target="_blank">Xem</a>`
                : ""}
            </td>
            <td>
                <button onclick="deleteCongDan('${cd.publicId}')">Xóa</button>
            </td>
        `;

        tbody.appendChild(tr);
    });

    total.innerText = `Tổng: ${data.length}`;
}

// =======================
// FORMAT DATE
// =======================
function formatDate(dateStr) {
    if (!dateStr) return "";
    return new Date(dateStr).toLocaleDateString("vi-VN");
}

// =======================
// DELETE
// =======================
window.deleteCongDan = async function (id) {
    if (!confirm("Xóa công dân?")) return;

    await del(`/CongDan/${id}`);
    loadData();
};