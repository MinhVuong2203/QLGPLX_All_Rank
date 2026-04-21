// ============================================
//  GPLX Manager – site.js
//  Theme, sidebar, nạp component, tích hợp API
// ============================================

(function () {
    'use strict';

    // ── Cấu hình ───────────────────────────────
    const API_BASE_URL = 'https://localhost:/api'; // Thay đổi theo API endpoint của bạn
    const THEME_KEY = 'gplx-theme';
    const SIDEBAR_KEY = 'gplx-sidebar';

    // Các asset tách theo từng route để dễ quản lý
    // (route nào không có trong map thì không nạp gì thêm)
    const PAGE_ASSETS = {
        list_gplx: {
            css: 'css/pages/list_gplx.css',
            js: 'js/pages/list_gplx.js'
        }
    };

    let currentRoute = null;
    const loadedPageScripts = new Set();

    const DEFAULT_ROUTE = 'dashboard';

    const html = document.documentElement;
    const shell = document.querySelector('.app-shell');

    // ── Loading toàn cục (overlay) ────────────
    let loadingDepth = 0;

    function setLoadingVisible(visible, message) {
        const overlay = document.getElementById('appLoading');
        if (!overlay) return;

        if (visible) overlay.removeAttribute('hidden');
        else overlay.setAttribute('hidden', '');

        const textEl = document.getElementById('appLoadingText');
        if (textEl && message) textEl.textContent = message;
    }

    function beginLoading(message) {
        loadingDepth += 1;
        setLoadingVisible(true, message || 'Đang tải…');
    }

    function endLoading() {
        loadingDepth = Math.max(0, loadingDepth - 1);
        if (loadingDepth === 0) setLoadingVisible(false);
    }

    // Export loading để các file JS riêng theo route có thể dùng
    window.GPLX = window.GPLX || {};
    window.GPLX.loading = {
        begin: beginLoading,
        end: endLoading
    };

    // ── Quản lý theme ─────────────────────────
    function getTheme() {
        return localStorage.getItem(THEME_KEY) ||
            (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light');
    }

    function applyTheme(theme) {
        html.setAttribute('data-theme', theme);
        localStorage.setItem(THEME_KEY, theme);
    }

    // Áp dụng theme ngay lập tức
    applyTheme(getTheme());

    // ── Quản lý sidebar ───────────────────────
    function getSidebarState() {
        return localStorage.getItem(SIDEBAR_KEY) === 'collapsed';
    }

    function applySidebar(collapsed) {
        if (collapsed) {
            shell?.classList.add('sidebar-collapsed');
        } else {
            shell?.classList.remove('sidebar-collapsed');
        }
        localStorage.setItem(SIDEBAR_KEY, collapsed ? 'collapsed' : 'open');
    }

    // ── Nạp component HTML ────────────────────
    async function loadComponent(selector, path, options) {
        const opts = options || {};
        const showLoading = Boolean(opts.showLoading);
        const loadingMessage = opts.loadingMessage;

        try {
            if (showLoading) beginLoading(loadingMessage);
            const response = await fetch(path);
            if (!response.ok) throw new Error(`Failed to load ${path}`);
            const html = await response.text();
            const element = document.querySelector(selector);
            if (element) {
                if (selector === '#pageContainer') element.setAttribute('aria-busy', 'true');
                element.innerHTML = html;
                if (selector === '#pageContainer') element.setAttribute('aria-busy', 'false');
                return true;
            }
            return false;
        } catch (error) {
            console.error(`Error loading component ${path}:`, error);
            return false;
        } finally {
            if (showLoading) endLoading();
        }
    }

    function setCurrentYear() {
        const yearEl = document.getElementById('currentYear');
        if (yearEl) yearEl.textContent = String(new Date().getFullYear());
    }

    // ── Router SPA (dựa trên hash) ─────────────
    function getRouteFromHash() {
        const raw = (window.location.hash || '').trim();
        if (!raw) return DEFAULT_ROUTE;
        const cleaned = raw.replace(/^#\/?/, '');
        return cleaned || DEFAULT_ROUTE;
    }

    function setDocumentTitle(route) {
        const titles = {
            dashboard: 'Tổng quan – GPLX Manager',
            statistics: 'Thống kê – GPLX Manager',
            list_gplx: 'Quản lý GPLX – GPLX Manager',
        };
        document.title = titles[route] || 'GPLX Manager';
    }

    function setBreadcrumb(route) {
        const labels = {
            dashboard: 'Trang chủ',
            statistics: 'Thống kê',
            list_gplx: 'Quản lý GPLX',
            'cap-moi': 'Cấp mới GPLX',
            'gia-han': 'Gia hạn GPLX',
            'thu-hoi': 'Thu hồi GPLX',
            'sat-hach': 'Kỳ sát hạch',
            'thi-sinh': 'Danh sách thí sinh',
            'bao-cao': 'Báo cáo',
            'cai-dat': 'Cài đặt',
            'tra-cuu': 'Tra cứu GPLX'
        };
        const breadcrumbText = document.querySelector('.breadcrumb-text');
        if (breadcrumbText) breadcrumbText.textContent = labels[route] || 'Trang';
    }

    function setActiveNav(route) {
        const navItems = document.querySelectorAll('.nav-item[data-route]');
        navItems.forEach(function (item) {
            const r = item.getAttribute('data-route');
            if (r === route) item.classList.add('active');
            else item.classList.remove('active');
        });
    }

    async function loadRoute(route) {
        const container = document.getElementById('pageContainer');
        if (!container) return;

        const routeToPath = {
            dashboard: 'pages/dashboard.html'
        };

        const path = routeToPath[route] || `pages/${route}.html`;

        try {
            const ok = await loadComponent('#pageContainer', path, {
                showLoading: true,
                loadingMessage: 'Đang tải trang…'
            });
            if (!ok) throw new Error('Missing #pageContainer');
        } catch (error) {
            console.warn(`Route load failed (${route}):`, error);
            container.innerHTML = `
                <h1 class="page-title">${route}</h1>
                <p class="page-subtitle">Trang này chưa có nội dung (tạo file <strong>${path}</strong>).</p>
            `;
        }

        // Nạp CSS/JS tách riêng theo route
        if (currentRoute !== route) currentRoute = route;
        await ensurePageAssets(route);

        // Khởi tạo riêng theo từng trang (phụ thuộc HTML vừa được inject)
        if (document.getElementById('statsContainer')) {
            loadDashboardData();
        }

        // Khởi tạo module trang (nếu có)
        tryInitPageModule(route);
    }

    function handleRouteChange() {
        const route = getRouteFromHash();
        setDocumentTitle(route);
        setBreadcrumb(route);
        setActiveNav(route);
        loadRoute(route);
    }

    function navigate(route) {
        window.location.hash = `#/${route}`;
    }

    // ── Nạp CSS/JS theo route ─────────────────
    function setPageCss(route, href) {
        const existing = document.getElementById('pageRouteCss');
        if (existing) {
            if (existing.dataset.route === route) return;
            existing.remove();
        }

        if (!href) return;
        const link = document.createElement('link');
        link.id = 'pageRouteCss';
        link.rel = 'stylesheet';
        link.href = href;
        link.dataset.route = route;
        document.head.appendChild(link);
    }

    function ensurePageScript(route, src) {
        if (!src) return Promise.resolve(true);
        if (loadedPageScripts.has(route)) return Promise.resolve(true);

        return new Promise(function (resolve) {
            const script = document.createElement('script');
            script.src = src;
            script.async = true;
            script.dataset.route = route;

            script.onload = function () {
                loadedPageScripts.add(route);
                resolve(true);
            };

            script.onerror = function () {
                console.warn(`Không nạp được script cho route '${route}': ${src}`);
                resolve(false);
            };

            document.body.appendChild(script);
        });
    }

    async function ensurePageAssets(route) {
        const assets = PAGE_ASSETS[route];

        // Dọn CSS route cũ (tránh ảnh hưởng trang khác)
        setPageCss(route, assets?.css);

        // Nạp JS route (nếu có)
        await ensurePageScript(route, assets?.js);
    }

    function tryInitPageModule(route) {
        const mod = window.GPLX?.pages?.[route];
        if (mod && typeof mod.init === 'function') {
            mod.init();
        }
    }

    // ── Tiện ích render an toàn ───────────────
    function escapeHtml(value) {
        return String(value ?? '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    // Export utils dùng chung
    window.GPLX = window.GPLX || {};
    window.GPLX.utils = window.GPLX.utils || {};
    window.GPLX.utils.escapeHtml = escapeHtml;

    // ── Hàm tiện ích gọi API ───────────────────
    const API = {
        async get(endpoint) {
            try {
                const response = await fetch(`${API_BASE_URL}${endpoint}`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        // Thêm token nếu API của bạn yêu cầu
                        // 'Authorization': `Bearer ${localStorage.getItem('token')}`
                    }
                });
                if (!response.ok) throw new Error(`API Error: ${response.status}`);
                return await response.json();
            } catch (error) {
                console.error('API GET Error:', error);
                throw error;
            }
        },

        async post(endpoint, data) {
            try {
                const response = await fetch(`${API_BASE_URL}${endpoint}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(data)
                });
                if (!response.ok) throw new Error(`API Error: ${response.status}`);
                return await response.json();
            } catch (error) {
                console.error('API POST Error:', error);
                throw error;
            }
        },

        async delete(endpoint) {
            try {
                const response = await fetch(`${API_BASE_URL}${endpoint}`, {
                    method: 'DELETE',
                    headers: {
                        'Content-Type': 'application/json',
                    }
                });
                if (!response.ok) throw new Error(`API Error: ${response.status}`);
                return await response.json();
            } catch (error) {
                console.error('API DELETE Error:', error);
                throw error;
            }
        }
    };

    // Export API để các file JS riêng theo route có thể dùng
    window.GPLX = window.GPLX || {};
    window.GPLX.api = API;

    // ── Khởi tạo ứng dụng ─────────────────────
    async function initializeApp() {
        // Nạp các component layout
        beginLoading('Đang khởi tạo…');
        await Promise.all([
            loadComponent('#headerContainer', 'components/header.html'),
            loadComponent('#footerContainer', 'components/footer.html')
        ]);
        endLoading();

        setCurrentYear();

        // Áp dụng trạng thái đã lưu
        applySidebar(getSidebarState());

        // Gắn event listener (chỉ 1 lần)
        setupEventListeners();

        // Tải thông tin user (header chắc chắn đã có sau khi nạp component)
        if (document.getElementById('userName')) loadUserInfo();

        // Khởi động router SPA
        window.addEventListener('hashchange', handleRouteChange);
        if (!window.location.hash) {
            window.location.hash = `#/${DEFAULT_ROUTE}`;
        } else {
            handleRouteChange();
        }
    }

    // ── Gắn event listeners ───────────────────
    function setupEventListeners() {
        // Nút đổi theme
        const themeToggle = document.getElementById('themeToggle');
        if (themeToggle) {
            themeToggle.addEventListener('click', function () {
                const current = html.getAttribute('data-theme') || 'light';
                applyTheme(current === 'light' ? 'dark' : 'light');
            });
        }

        // Nút thu gọn/mở rộng sidebar
        const sidebarToggle = document.getElementById('sidebarToggle');
        if (sidebarToggle) {
            sidebarToggle.addEventListener('click', function () {
                const isCollapsed = shell?.classList.contains('sidebar-collapsed');
                applySidebar(!isCollapsed);
            });
        }

        // Điều hướng SPA cho các phần tử nằm trong trang được inject
        document.addEventListener('click', function (e) {
            const routeEl = e.target.closest('[data-route]');
            if (routeEl && !routeEl.classList.contains('nav-item')) {
                const route = routeEl.getAttribute('data-route');
                if (route) {
                    e.preventDefault();
                    navigate(route);
                }
            }
        });

        // Phím tắt tìm kiếm (Cmd/Ctrl + K)
        document.addEventListener('keydown', function (e) {
            if ((e.metaKey || e.ctrlKey) && e.key === 'k') {
                e.preventDefault();
                const searchInput = document.querySelector('.search-input');
                if (searchInput) {
                    searchInput.focus();
                    searchInput.select();
                }
            }
        });

        // Nút đăng xuất
        const logoutBtn = document.getElementById('logoutBtn');
        if (logoutBtn) {
            logoutBtn.addEventListener('click', function (e) {
                e.preventDefault();
                handleLogout();
            });
        }

        // Mobile: bấm ra ngoài để đóng sidebar overlay
        document.addEventListener('click', function (e) {
            if (window.innerWidth <= 768) {
                const sidebar = document.querySelector('.sidebar');
                const toggle = document.getElementById('sidebarToggle');
                if (shell?.classList.contains('sidebar-open') &&
                    sidebar && !sidebar.contains(e.target) &&
                    toggle && !toggle.contains(e.target)) {
                    shell.classList.remove('sidebar-open');
                }
            }
        });

        // Mobile: xử lý nút mở/đóng sidebar
        if (window.innerWidth <= 768) {
            const sidebarToggleMobile = document.getElementById('sidebarToggle');
            if (sidebarToggleMobile) {
                sidebarToggleMobile.addEventListener('click', function () {
                    shell?.classList.toggle('sidebar-open');
                });
            }
        }
    }

    // ── Tải thông tin user từ API ─────────────
    async function loadUserInfo() {
        try {
            // Ví dụ gọi API - chỉnh endpoint theo dự án của bạn
            const userData = await API.get('/auth/user');

            // Cập nhật tên
            const userNameElements = document.querySelectorAll('#userName, .sidebar-user-name');
            userNameElements.forEach(el => {
                if (el) el.textContent = userData.name || 'Admin';
            });

            // Cập nhật vai trò
            const userRoleElements = document.querySelectorAll('#userRole, .sidebar-user-role');
            userRoleElements.forEach(el => {
                if (el) el.textContent = userData.role || 'Quản trị viên';
            });

            // Cập nhật chữ cái đại diện
            const userInitialElements = document.querySelectorAll('#userInitial, .sidebar-user-avatar');
            userInitialElements.forEach(el => {
                if (el && userData.name) {
                    el.textContent = userData.name.charAt(0).toUpperCase();
                }
            });
        } catch (error) {
            console.error('Failed to load user info:', error);
            // Nếu lỗi thì giữ giá trị mặc định
        }
    }

    // ── Tải dữ liệu dashboard ────────────────
    async function loadDashboardData() {
        try {
            // Ví dụ: tải thống kê
            const stats = await API.get('/dashboard/stats');
            updateStatCards(stats);

            // Ví dụ: tải danh sách GPLX gần đây
            const recentLicenses = await API.get('/licenses/recent?limit=5');
            updateLicenseTable(recentLicenses);
        } catch (error) {
            console.error('Failed to load dashboard data:', error);
        }
    }

    // ── Cập nhật thẻ thống kê ─────────────────
    function updateStatCards(stats) {
        if (!stats) return;

        // Ví dụ mapping - chỉnh theo cấu trúc response API của bạn
        const statMappings = [
            { key: 'totalActiveLicenses', selector: '[data-stat="total-active"] .stat-value' },
            { key: 'newThisMonth', selector: '[data-stat="new-month"] .stat-value' },
            { key: 'expiringSoon', selector: '[data-stat="expiring"] .stat-value' },
            { key: 'revoked', selector: '[data-stat="revoked"] .stat-value' }
        ];

        statMappings.forEach(mapping => {
            const element = document.querySelector(mapping.selector);
            if (element && stats[mapping.key] !== undefined) {
                element.textContent = stats[mapping.key].toLocaleString('vi-VN');
            }
        });
    }

    // ── Cập nhật bảng GPLX ────────────────────
    function updateLicenseTable(licenses) {
        const tbody = document.querySelector('.data-table tbody');
        if (!tbody || !licenses || !Array.isArray(licenses)) return;

        tbody.innerHTML = licenses.map(license => `
            <tr>
                <td><strong>${license.licenseNumber || 'N/A'}</strong></td>
                <td>${license.fullName || 'N/A'}</td>
                <td>${license.class || 'N/A'}</td>
                <td>${formatDate(license.issueDate)}</td>
                <td>${formatDate(license.expiryDate)}</td>
                <td><span class="badge badge-${getStatusBadgeClass(license.status)}">${getStatusText(license.status)}</span></td>
            </tr>
        `).join('');
    }

    // ── Hàm dùng chung ────────────────────────
    function formatDate(dateString) {
        if (!dateString) return '-';
        const date = new Date(dateString);
        return date.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit', year: 'numeric' });
    }

    function getStatusBadgeClass(status) {
        const statusMap = {
            'active': 'success',
            'expiring': 'warning',
            'expired': 'danger',
            'revoked': 'danger'
        };
        return statusMap[status?.toLowerCase()] || 'success';
    }

    function getStatusText(status) {
        const textMap = {
            'active': 'Hiệu lực',
            'expiring': 'Sắp hết hạn',
            'expired': 'Hết hạn',
            'revoked': 'Đã thu hồi'
        };
        return textMap[status?.toLowerCase()] || 'Hiệu lực';
    }

    // ── Xử lý đăng xuất ───────────────────────
    async function handleLogout() {
        try {
            // Gọi API logout nếu cần
            // await API.post('/auth/logout');

            // Xoá dữ liệu local storage
            localStorage.removeItem('token');
            localStorage.removeItem('user');

            // Chuyển về trang đăng nhập
            window.location.href = 'login.html';
        } catch (error) {
            console.error('Logout error:', error);
            // Dù lỗi vẫn chuyển trang
            window.location.href = 'login.html';
        }
    }

    // ── Public API: đưa ra global để script khác dùng ──
    window.GPLX = {
        API: API,
        loadComponent: loadComponent,
        applyTheme: applyTheme,
        getTheme: getTheme,
        navigate: navigate
    };

    // ── Chạy ứng dụng ─────────────────────────
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initializeApp);
    } else {
        initializeApp();
    }

})();