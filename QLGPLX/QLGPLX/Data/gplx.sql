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
    LoaiXe VARCHAR(255),
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
    diem_dat INT NOT NULL,
	diem_toi_da INT NOT NULL,
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
    FOREIGN KEY (MaHang) REFERENCES HangGiayPhep(MaHang)
);

-- ================== KẾT QUẢ ==================
CREATE TABLE KetQuaThi (
    KetQuaID INT AUTO_INCREMENT PRIMARY KEY,
    HoSoID INT NOT NULL,
    KyThiID INT NOT NULL,
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
    KetQuaID INT NOT NULL,
    MonThiID INT NOT NULL,
    Diem INT NOT NULL DEFAULT 0,
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
    TrangThai VARCHAR(30) DEFAULT 'Còn hiệu lực', -- Chờ duyệt, còn hiệu lực, 
    -- hết hạn, bị thu hồi sẽ do ứng dụng tính toán
    GhiChu VARCHAR(255),
    FOREIGN KEY (MaCongDan) REFERENCES CongDan(MaCongDan),
    FOREIGN KEY (MaHang) REFERENCES HangGiayPhep(MaHang)
);
-- ============= LỊCH SỬ GPLX =========== -
CREATE TABLE LichSuGiayPhep (
    LichSuID INT AUTO_INCREMENT PRIMARY KEY,
    GiayPhepID INT NOT NULL,
    LoaiThaoTac VARCHAR(30) NOT NULL, -- CAP_MOI, GIA_HAN, CAP_LAI, THU_HOI
    SoGiayPhep VARCHAR(20) NOT NULL,
    NgayCapCu DATE,
    NgayHetHanCu DATE,
    NgayCapMoi DATE,
    NgayHetHanMoi DATE,
    LyDo VARCHAR(255),
    NgayThucHien DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (GiayPhepID) REFERENCES GiayPhep(GiayPhepID)
);

-- ================== CHỨC VỤ ==================
CREATE TABLE ChucVu (
    MaChucVu INT AUTO_INCREMENT PRIMARY KEY,
    TenChucVu VARCHAR(50) UNIQUE
);
INSERT INTO ChucVu (TenChucVu)
VALUES ('Quản lý'), ('Cán bộ hồ sơ'), ('Cán bộ sát hạch'), ('Cán bộ cấp GPLX');

-- ================== CÁN BỘ ==================
CREATE TABLE CanBo (
    MaCanBo INT AUTO_INCREMENT PRIMARY KEY,
    public_id CHAR(36) UNIQUE NOT NULL,
    HoTen VARCHAR(100),
    MaChucVu INT,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Cccd VARCHAR(12) UNIQUE NOT NULL,
    DienThoai VARCHAR(15),
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP,
    Anh3x4 VARCHAR(256),
    Username VARCHAR(100) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    TrangThai TINYINT(1) DEFAULT 1,
    FOREIGN KEY (MaChucVu) REFERENCES ChucVu(MaChucVu)
);

CREATE TABLE PasswordResetOTP (
    OTPID INT AUTO_INCREMENT PRIMARY KEY,
    MaCanBo INT NOT NULL,
    OTPCode VARCHAR(10) NOT NULL,
    ExpiredAt DATETIME NOT NULL,
    IsUsed TINYINT(1) DEFAULT 0,
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (MaCanBo) REFERENCES CanBo(MaCanBo)
);
-- Chức năng
CREATE TABLE ChucNang (
    MaChucNang INT AUTO_INCREMENT PRIMARY KEY,
    TenChucNang VARCHAR(100) NOT NULL,
    MaChucNangCode VARCHAR(100) UNIQUE NOT NULL,
    MoTa VARCHAR(255),
    TrangThai TINYINT(1) DEFAULT 1
);
INSERT INTO ChucNang (TenChucNang, MaChucNangCode, MoTa)
VALUES
('Quản lý cán bộ', 'QUAN_LY_CAN_BO', 'Thêm, sửa, xóa cán bộ'),
('Quản lý hồ sơ đăng ký', 'QUAN_LY_HO_SO', 'Xem và xử lý hồ sơ đăng ký'),
('Duyệt hồ sơ', 'DUYET_HO_SO', 'Duyệt hồ sơ đủ điều kiện thi'),
('Quản lý kỳ thi', 'QUAN_LY_KY_THI', 'Tạo và quản lý kỳ thi sát hạch'),
('Nhập kết quả thi', 'NHAP_KET_QUA_THI', 'Nhập kết quả sát hạch'),
('Cấp GPLX', 'CAP_GPLX', 'Cấp giấy phép lái xe'),
('Gia hạn GPLX', 'GIA HAN_GPLX', 'Gia hạn giấy phép lái xe'),
('Đăng nhập', 'LOGIN', 'Đăng nhập hệ thống');

CREATE TABLE PhanQuyenCanBo (
    MaCanBo INT NOT NULL,
    MaChucNang INT NOT NULL,
    PRIMARY KEY (MaCanBo, MaChucNang),
    FOREIGN KEY (MaCanBo) REFERENCES CanBo(MaCanBo),
    FOREIGN KEY (MaChucNang) REFERENCES ChucNang(MaChucNang)
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

INSERT INTO hang_mon_thi 
(ma_hang, mon_thiid, diem_dat, diem_toi_da)
VALUES
-- A1
('A1', 1, 21, 25),
('A1', 3, 80, 100),
-- A
('A', 1, 23, 25),
('A', 3, 80, 100),
-- B1
('B1', 1, 23, 25),
('B1', 3, 80, 100),
-- B
('B', 1, 27, 30),
('B', 2, 35, 50),
('B', 3, 80, 100),
('B', 4, 80, 100),
-- C1
('C1', 1, 32, 35),
('C1', 2, 35, 50),
('C1', 3, 80, 100),
('C1', 4, 80, 100),
-- C
('C', 1, 36, 40),
('C', 2, 35, 50),
('C', 3, 80, 100),
('C', 4, 80, 100),
-- D1
('D1', 1, 41, 45),
('D1', 2, 35, 50),
('D1', 3, 80, 100),
('D1', 4, 80, 100),
-- D2
('D2', 1, 41, 45),
('D2', 2, 35, 50),
('D2', 3, 80, 100),
('D2', 4, 80, 100),
-- D
('D', 1, 41, 45),
('D', 2, 35, 50),
('D', 3, 80, 100),
('D', 4, 80, 100),
-- BE
('BE', 1, 41, 45),
('BE', 2, 35, 50),
('BE', 3, 80, 100),
('BE', 4, 80, 100),
-- C1E
('C1E', 1, 41, 45),
('C1E', 2, 35, 50),
('C1E', 3, 80, 100),
('C1E', 4, 80, 100),
-- CE
('CE', 1, 41, 45),
('CE', 2, 35, 50),
('CE', 3, 80, 100),
('CE', 4, 80, 100),
-- D1E
('D1E', 1, 41, 45),
('D1E', 2, 35, 50),
('D1E', 3, 80, 100),
('D1E', 4, 80, 100),
-- D2E
('D2E', 1, 41, 45),
('D2E', 2, 35, 50),
('D2E', 3, 80, 100),
('D2E', 4, 80, 100),
-- DE
('DE', 1, 41, 45),
('DE', 2, 35, 50),
('DE', 3, 80, 100),
('DE', 4, 80, 100);

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

DROP trigger  trg_ketquachitiet_after_insert_ketquathi
-- Trigger tự động tạo các kết quả thành phần cho Lần thi 1
DELIMITER $$

CREATE TRIGGER trg_ketquachitiet_after_insert_ketquathi
AFTER INSERT ON KetQuaThi
FOR EACH ROW
BEGIN

    IF NEW.LanThi = 1 THEN

        INSERT INTO KetQuaChiTiet
        (
            KetQuaID,
            MonThiID,
            Diem,
            ThoiGianBatDau,
            KetQua,
            GhiChu
        )
        SELECT
            NEW.KetQuaID,
            hmt.mon_thiid,
            0,
            NULL,
            'Chưa thi',
            NULL
        FROM hang_mon_thi hmt
        INNER JOIN KyThi kt
            ON kt.MaHang = hmt.ma_hang
        WHERE kt.KyThiID = NEW.KyThiID;

    END IF;

END$$

DELIMITER ;

-- =========================================
-- INSERT bảng ketquathi nếu Đạt thì tạo giấy phép
-- =========================================
DROP TRIGGER trg_ketquathi_insert_giayphep_insert
DROP TRIGGER trg_ketquathi_update_giayphep_insert
DELIMITER $$
CREATE TRIGGER trg_ketquathi_insert_giayphep_insert
AFTER INSERT ON KetQuaThi
FOR EACH ROW
BEGIN
    DECLARE v_maCongDan INT;
    DECLARE v_maHang VARCHAR(10);

    -- Chỉ xử lý nếu Đạt
    IF NEW.KetQuaTongHop = 'Đạt' THEN

        -- Lấy thông tin từ hồ sơ
        SELECT hs.MaCongDan, hs.MaHang
        INTO v_maCongDan, v_maHang
        FROM HoSo hs
        WHERE hs.HoSoID = NEW.HoSoID;

        -- Chỉ thêm nếu chưa có GPLX
        IF NOT EXISTS (
            SELECT 1
            FROM GiayPhep gp
            WHERE gp.MaCongDan = v_maCongDan
              AND gp.MaHang = v_maHang
        ) THEN

            INSERT INTO GiayPhep (
                MaCongDan,
                MaHang,
                SoGiayPhep,
                NgayCap,
                NgayHetHan,
                TrangThai
            )
            VALUES (
                v_maCongDan,
                v_maHang,
                CONCAT(
                    'GPLX-',
                    v_maCongDan,
                    '-',
                    UNIX_TIMESTAMP()
                ),
                CURDATE(),
                DATE_ADD(CURDATE(), INTERVAL 10 YEAR),
                'Chờ duyệt'
            );

        END IF;
    END IF;
END$$


-- =========================================
-- UPDATE ketquathi Đạt thì thêm giấy phép
-- =========================================
DROP TRIGGER IF EXISTS trg_ketquathi_update_giayphep_insert;

DELIMITER $$

CREATE TRIGGER trg_ketquathi_update_giayphep_insert
AFTER UPDATE ON KetQuaThi
FOR EACH ROW
BEGIN
    DECLARE v_maCongDan INT;
    DECLARE v_maHang VARCHAR(10);

    DECLARE v_gioiTinh VARCHAR(10);
    DECLARE v_namSinh YEAR;

    DECLARE v_maGioiTinh CHAR(1);
    DECLARE v_namTrungTuyen CHAR(2);

    DECLARE v_random7 VARCHAR(7);

    DECLARE v_soGPLX VARCHAR(20);

    DECLARE v_thoiHanNam INT;
    DECLARE v_ngayHetHan DATE;

    -- Lấy thông tin công dân + hồ sơ
    SELECT 
        hs.MaCongDan,
        hs.MaHang,
        cd.GioiTinh,
        YEAR(cd.NgaySinh)
    INTO 
        v_maCongDan,
        v_maHang,
        v_gioiTinh,
        v_namSinh
    FROM HoSo hs
    INNER JOIN CongDan cd 
        ON cd.MaCongDan = hs.MaCongDan
    WHERE hs.HoSoID = NEW.HoSoID;
    -- Xác định mã giới tính
    /*
        Thế kỷ 20:
            Nam = 0
            Nữ = 1
        Thế kỷ 21:
            Nam = 2
            Nữ = 3
    */
    IF v_namSinh BETWEEN 1900 AND 1999 THEN

        IF LOWER(v_gioiTinh) = 'nam' THEN
            SET v_maGioiTinh = '0';
        ELSE
            SET v_maGioiTinh = '1';
        END IF;
    ELSE
        IF LOWER(v_gioiTinh) = 'nam' THEN
            SET v_maGioiTinh = '2';
        ELSE
            SET v_maGioiTinh = '3';
        END IF;

    END IF;
    -- Năm trúng tuyển
    -- Ví dụ 2026 => 26

    SET v_namTrungTuyen = RIGHT(YEAR(CURDATE()), 2);

    SET v_random7 = LPAD(
        FLOOR(RAND() * 10000000),
        7,
        '0'
    );


    SET v_soGPLX = CONCAT(
        '89',
        v_maGioiTinh,
        v_namTrungTuyen,
        v_random7
    );


    IF OLD.KetQuaTongHop <> 'Đạt'
       AND NEW.KetQuaTongHop = 'Đạt' THEN

        IF NOT EXISTS (
            SELECT 1
            FROM GiayPhep gp
            WHERE gp.MaCongDan = v_maCongDan
              AND gp.MaHang = v_maHang
        ) THEN

            -- Lấy thời hạn GPLX
 
            SELECT h.ThoiHanNam
            INTO v_thoiHanNam
            FROM HangGiayPhep h
            WHERE h.MaHang = v_maHang;

            -- Nếu không thời hạn => NULL

            IF v_thoiHanNam IS NULL THEN

                SET v_ngayHetHan = NULL;

            ELSE

                SET v_ngayHetHan = DATE_ADD(
                    CURDATE(),
                    INTERVAL v_thoiHanNam YEAR
                );

            END IF;

            -- Insert giấy phép

            INSERT INTO GiayPhep (
                MaCongDan,
                MaHang,
                SoGiayPhep,
                NgayCap,
                NgayHetHan,
                TrangThai
            )
            VALUES (
                v_maCongDan,
                v_maHang,
                v_soGPLX,
                CURDATE(),
                v_ngayHetHan,
                'Chờ duyệt'
            );

        END IF;

    END IF;
    
    -- Từ ĐẠT -> KHÔNG ĐẠT

    IF OLD.KetQuaTongHop = 'Đạt'
       AND NEW.KetQuaTongHop <> 'Đạt' THEN

        DELETE FROM GiayPhep
        WHERE MaCongDan = v_maCongDan
          AND MaHang = v_maHang;
    END IF;

END$$

DELIMITER ;








