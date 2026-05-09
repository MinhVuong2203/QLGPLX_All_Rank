CREATE DATABASE IF NOT EXISTS qlgplx;
USE qlgplx;

-- ================== CÔNG DÂN ==================
CREATE TABLE CongDan (
    MaCongDan INT AUTO_INCREMENT PRIMARY KEY,
    public_id CHAR(36) UNIQUE, 
    HoTen VARCHAR(100) NOT NULL,
    NgaySinh DATE NOT NULL,
    GioiTinh VARCHAR(10),
    CCCD VARCHAR(20) UNIQUE NOT NULL,
    DiaChi VARCHAR(255),
    SoDienThoai VARCHAR(15) UNIQUE,
    Email VARCHAR(100) UNIQUE,
    TinhTrangSucKhoe VARCHAR(50) DEFAULT 'Khỏe mạnh',
    NgayKhamSucKhoe DATE,
    GiayKhamSucKhoe VARCHAR(255),
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP,
    Anh3x4 VARCHAR(255)
);



-- ================== HẠNG GPLX ==================
CREATE TABLE HangGiayPhep (
    MaHang VARCHAR(10) PRIMARY KEY,
    TenHang VARCHAR(50) NOT NULL,
    LoaiXe VARCHAR(50),
    DoTuoiToiThieu INT DEFAULT 18,
    ThoiHanNam INT,
    YeuCauThucHanh TINYINT(1) DEFAULT 1,
    MoTaChiTiet TEXT
);

-- ================== MÔN THI ==================
CREATE TABLE MonThi (
    MonThiID INT AUTO_INCREMENT PRIMARY KEY,
    TenMon VARCHAR(100) NOT NULL,
    MoTa VARCHAR(255)
);

-- ================== HẠNG - MÔN THI ==================
DROP TABLE IF EXISTS hang_mon_thi;

CREATE TABLE hang_mon_thi (
    ma_hang VARCHAR(10) NOT NULL,
    mon_thiid INT NOT NULL,
    diem_dat DECIMAL(5,2) NOT NULL,
    PRIMARY KEY (ma_hang, mon_thiid),
    FOREIGN KEY (ma_hang) REFERENCES HangGiayPhep(MaHang),
    FOREIGN KEY (mon_thiid) REFERENCES MonThi(MonThiID)
);

-- ================== HỒ SƠ ==================
CREATE TABLE HoSo (
    HoSoID INT AUTO_INCREMENT PRIMARY KEY,
    public_id CHAR(36) UNIQUE,
    MaCongDan INT NOT NULL,
    MaHang VARCHAR(10) NOT NULL,
    NgayNop DATETIME DEFAULT CURRENT_TIMESTAMP,
    TrangThai VARCHAR(30) DEFAULT 'Đang xử lý', -- Đang xử lý, Đã duyệt, Từ chối, hoàn thành, đã cấp  
    TrangThaiThanhToan TINYINT(1) DEFAULT 0,
    GhiChu VARCHAR(255),
    FOREIGN KEY (MaCongDan) REFERENCES CongDan(MaCongDan),
    FOREIGN KEY (MaHang) REFERENCES HangGiayPhep(MaHang)
);

-- ================== KỲ THI ==================
CREATE TABLE KyThi (
    KyThiID INT AUTO_INCREMENT PRIMARY KEY,
    public_id CHAR(36) UNIQUE,
    TenKyThi VARCHAR(150),
    NgayBatDau DATE,
    NgayKetThuc DATE,
    DiaDiem VARCHAR(255),
    MaHang VARCHAR(10),
    SoLuongToiDa INT DEFAULT 100,
    SoLuongDangKy INT DEFAULT 0,
    TrangThai VARCHAR(30) DEFAULT 'Sắp diễn ra',
    FOREIGN KEY (MaHang) REFERENCES HangGiayPhep(MaHang)
);
-- Trạng thái tự động
ALTER TABLE KyThi
ADD TrangThai VARCHAR(30)
GENERATED ALWAYS AS (
    CASE
        WHEN CURDATE() < NgayBatDau THEN 'Sắp diễn ra'
        WHEN CURDATE() BETWEEN NgayBatDau AND NgayKetThuc THEN 'Đang diễn ra'
        ELSE 'Đã kết thúc'
    END
) STORED;

-- ================== LỊCH THI ==================
CREATE TABLE LichThi (
    LichThiID INT AUTO_INCREMENT PRIMARY KEY,
    KyThiID INT,
    MonThiID INT,
    ThoiGian DATETIME,
    FOREIGN KEY (KyThiID) REFERENCES KyThi(KyThiID),
    FOREIGN KEY (MonThiID) REFERENCES MonThi(MonThiID)
);

-- ================== KẾT QUẢ ==================
CREATE TABLE KetQuaThi (
    KetQuaID INT AUTO_INCREMENT PRIMARY KEY,
    HoSoID INT,
    KyThiID INT,
    KetQuaTongHop VARCHAR(20),
    NgayKetLuan DATETIME DEFAULT CURRENT_TIMESTAMP,
    LanThi INT DEFAULT 1,
    GhiChu VARCHAR(255),
    UNIQUE (HoSoID, KyThiID, LanThi),
    FOREIGN KEY (HoSoID) REFERENCES HoSo(HoSoID),
    FOREIGN KEY (KyThiID) REFERENCES KyThi(KyThiID)
);

-- ================== KẾT QUẢ CHI TIẾT ==================
CREATE TABLE KetQuaChiTiet (
    ChiTietID INT AUTO_INCREMENT PRIMARY KEY,
    KetQuaID INT,
    MonThiID INT,
    Diem DECIMAL(5,2),
    ThoiGianBatDau DATETIME,
    KetQua VARCHAR(20),
    GhiChu VARCHAR(255),
    UNIQUE (KetQuaID, MonThiID),
    FOREIGN KEY (KetQuaID) REFERENCES KetQuaThi(KetQuaID) ON DELETE CASCADE,
    FOREIGN KEY (MonThiID) REFERENCES MonThi(MonThiID)
);

-- ================== GIẤY PHÉP ==================
CREATE TABLE GiayPhep (
    GiayPhepID INT AUTO_INCREMENT PRIMARY KEY,
    MaCongDan INT,
    MaHang VARCHAR(10),
    SoGiayPhep VARCHAR(20) UNIQUE,
    NgayCap DATE,
    NgayHetHan DATE,
    SoDiem INT DEFAULT 12,
    TrangThai VARCHAR(30) DEFAULT 'Còn hiệu lực',
    GhiChu VARCHAR(255),
    FOREIGN KEY (MaCongDan) REFERENCES CongDan(MaCongDan),
    FOREIGN KEY (MaHang) REFERENCES HangGiayPhep(MaHang)
);

-- ================== VI PHẠM ==================
CREATE TABLE LoaiViPham (
    LoaiViPhamID INT AUTO_INCREMENT PRIMARY KEY,
    TenViPham VARCHAR(255),
    DiemTru INT,
    MucPhatTu DECIMAL(18,2),
    MucPhatDen DECIMAL(18,2),
    MoTa VARCHAR(500)
);

CREATE TABLE ViPham (
    ViPhamID INT AUTO_INCREMENT PRIMARY KEY,
    GiayPhepID INT,
    LoaiViPhamID INT,
    ThoiGianViPham DATETIME DEFAULT CURRENT_TIMESTAMP,
    DiaDiem VARCHAR(255),
    BienKiemSoat VARCHAR(20),
    MucPhat DECIMAL(18,2),
    TrangThai VARCHAR(30) DEFAULT 'Chưa xử lý',
    GhiChu VARCHAR(500),
    FOREIGN KEY (GiayPhepID) REFERENCES GiayPhep(GiayPhepID),
    FOREIGN KEY (LoaiViPhamID) REFERENCES LoaiViPham(LoaiViPhamID)
);

-- ================== CHỨC VỤ ==================
CREATE TABLE ChucVu (
    MaChucVu INT AUTO_INCREMENT PRIMARY KEY,
    TenChucVu VARCHAR(50) UNIQUE
);

INSERT INTO ChucVu (TenChucVu)
VALUES ('Quản lý'), ('Cán bộ hồ sơ'), ('Cán bộ sát hạch'), ('Cán bộ cấp GPLX'), ('Cán bộ xử lý vi phạm');

-- ================== CÁN BỘ ==================
CREATE TABLE CanBo (
    MaCanBo INT AUTO_INCREMENT PRIMARY KEY,
    public_id CHAR(36) UNIQUE,
    HoTen VARCHAR(100),
    MaChucVu INT,
    Email VARCHAR(120),
    DienThoai VARCHAR(15),
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP,
    Username VARCHAR(100) UNIQUE,
    Password VARCHAR(100),
    Anh3x4 VARCHAR(256),
    TrangThai TINYINT(1) DEFAULT 1,
    FOREIGN KEY (MaChucVu) REFERENCES ChucVu(MaChucVu)
);

-- ================== CÁN BỘ - HỒ SƠ ==================
CREATE TABLE CanBo_HoSo (
    MaCanBo INT,
    HoSoID INT,
    ThoiGian DATETIME DEFAULT CURRENT_TIMESTAMP,
    TrangThaiDuyet VARCHAR(50),
    PRIMARY KEY (MaCanBo, HoSoID, ThoiGian),
    FOREIGN KEY (MaCanBo) REFERENCES CanBo(MaCanBo),
    FOREIGN KEY (HoSoID) REFERENCES HoSo(HoSoID)
);


INSERT INTO HangGiayPhep (MaHang, TenHang, LoaiXe, DoTuoiToiThieu, ThoiHanNam, YeuCauThucHanh, MoTaChiTiet) VALUES
('A1', 'Mô tô 2 bánh nhỏ', 'Xe máy ≤125cc', 18, null , 1, 'Mô tô hai bánh ≤125cc hoặc ≤11kW'), 
('A', 'Mô tô 2 bánh lớn', 'Xe máy >125cc', 18, null, 1, 'Mô tô hai bánh >125cc hoặc >11kW'),
('B1', 'Mô tô ba bánh', 'Xe 3 bánh', 18, null, 1, 'Xe mô tô ba bánh (luật mới 2025)'),

('B', 'Ô tô con', 'Ô tô ≤9 chỗ', 18, 10, 1, 'Ô tô ≤9 chỗ, tải ≤3500kg'),

('C1', 'Ô tô tải trung', 'Tải 3.5 - 7.5 tấn', 18, 5, 1, 'Ô tô tải >3500kg đến 7500kg'),
('C', 'Ô tô tải nặng', 'Tải >7.5 tấn', 21, 5, 1, 'Ô tô tải >7500kg'),

('D1', 'Ô tô khách nhỏ', '8-16 chỗ', 24, 5, 1, 'Xe >8 đến 16 chỗ'),
('D2', 'Ô tô khách trung', '16-29 chỗ', 24, 5, 1, 'Xe >16 đến 29 chỗ'),
('D', 'Ô tô khách lớn', '>29 chỗ', 27, 5, 1, 'Xe >29 chỗ, giường nằm'),

('BE', 'B kéo rơ-moóc', 'Ô tô + rơ-moóc', 21, 5, 1, 'Xe B kéo rơ-moóc >750kg'),
('C1E', 'C1 kéo rơ-moóc', 'Tải trung + rơ-moóc', 24, 5, 1, 'Xe C1 kéo rơ-moóc >750kg'),
('CE', 'C kéo rơ-moóc', 'Tải nặng + rơ-moóc', 24, 5, 1, 'Xe C kéo rơ-moóc, xe đầu kéo'),

('D1E', 'D1 kéo rơ-moóc', 'Khách nhỏ + rơ-moóc', 27, 5, 1, 'Xe D1 kéo rơ-moóc >750kg'),
('D2E', 'D2 kéo rơ-moóc', 'Khách trung + rơ-moóc', 27, 5, 1, 'Xe D2 kéo rơ-moóc >750kg'),
('DE', 'D kéo rơ-moóc', 'Khách lớn + rơ-moóc', 27, 5, 1, 'Xe D kéo rơ-moóc, xe nối toa');

INSERT INTO MonThi (TenMon) VALUES
('Lý thuyết'),
('Mô phỏng'),
('Sa hình'),
('Đường trường');

-- TRIGGER --
-- Tăng +1 vào SoLuongDangKy mỗi khi có insert KetQuaThi
-- (đồng nghĩa với việc thêm 1 hồ sơ vào Kỳ thi)
DELIMITER $$

CREATE TRIGGER trg_ketquathi_before_insert
BEFORE INSERT ON KetQuaThi
FOR EACH ROW
BEGIN
    DECLARE soDangKy INT;
    DECLARE soToiDa INT;

    -- chỉ tính lần thi đầu
    IF NEW.LanThi = 1 THEN

        SELECT SoLuongDangKy, SoLuongToiDa
        INTO soDangKy, soToiDa
        FROM KyThi
        WHERE KyThiID = NEW.KyThiID
        FOR UPDATE;

        -- ❌ nếu full thì chặn
        IF soDangKy >= soToiDa THEN
            SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Ky thi da du so luong';
        END IF;

    END IF;

END$$

DELIMITER ;

DELIMITER $$

CREATE TRIGGER trg_ketquathi_after_insert
AFTER INSERT ON KetQuaThi
FOR EACH ROW
BEGIN
    IF NEW.LanThi = 1 THEN
        UPDATE KyThi
        SET SoLuongDangKy = SoLuongDangKy + 1
        WHERE KyThiID = NEW.KyThiID;
    END IF;
END$$

DELIMITER ;

-- Giảm -1 vào SoLuongDangKy mỗi khi có insert KetQuaThi
-- (đồng nghĩa với việc xóa 1 hồ sơ vào Kỳ thi)
DELIMITER $$

CREATE TRIGGER trg_ketquathi_after_delete
AFTER DELETE ON KetQuaThi
FOR EACH ROW
BEGIN
    IF OLD.LanThi = 1 THEN
        UPDATE KyThi
        SET SoLuongDangKy = GREATEST(SoLuongDangKy - 1, 0)
        WHERE KyThiID = OLD.KyThiID;
    END IF;
END$$

DELIMITER ;






