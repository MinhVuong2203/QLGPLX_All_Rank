const BASE_URL = "https://localhost:7033/api";

export async function get(url) {
    const res = await fetch(BASE_URL + url);
    return res.json();
}

export async function del(url) {
    await fetch(BASE_URL + url, { method: "DELETE" });
}