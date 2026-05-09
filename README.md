# 🚗 HỆ THỐNG QUẢN LÝ GPLX – LUỒNG XỬ LÝ CHÍNH

## 🎯 Mục tiêu hệ thống

Hệ thống dùng để quản lý toàn bộ quy trình cấp Giấy phép lái xe (GPLX), bao gồm:

* Quản lý công dân
* Quản lý hồ sơ đăng ký
* Tổ chức kỳ thi
* Ghi nhận kết quả thi
* Cấp giấy phép
* Quản lý vi phạm sau khi cấp


## 🧠 Tổng quan luồng xử lý

Hệ thống hoạt động theo quy trình sau:

```
Công dân → Hồ sơ → Duyệt → Kỳ thi → Kết quả → GPLX → Vi phạm
```

## 📌 1. Quản lý công dân

* Tạo mới công dân
* Cập nhật thông tin cá nhân
* Lưu trữ:

  * Họ tên
  * CCCD
  * Ngày sinh
  * Sức khỏe

👉 Mỗi công dân có thể đăng ký nhiều hồ sơ thi khác nhau.


## 📌 2. Tạo hồ sơ đăng ký

* Công dân đăng ký thi GPLX
* Chọn hạng GPLX (A1, B2, C,...)

Thông tin lưu:

* Công dân
* Hạng đăng ký
* Ngày nộp
* Trạng thái hồ sơ

👉 Trạng thái ban đầu:

```
Đang xử lý
```


## 📌 3. Duyệt hồ sơ

Cán bộ kiểm tra:

* Độ tuổi
* Sức khỏe
* Điều kiện thi

👉 Cập nhật trạng thái:

* Đủ điều kiện
* Không đủ điều kiện

👉 Chỉ hồ sơ "Đủ điều kiện" mới được thi.



## 📌 4. Tổ chức kỳ thi

* Tạo kỳ thi theo từng hạng GPLX
* Thiết lập:

  * Tên kỳ thi
  * Ngày thi
  * Địa điểm

👉 Gán hồ sơ vào kỳ thi tương ứng.



## 📌 5. Nhập kết quả thi

Sau khi thi, cán bộ nhập kết quả:

### Gồm:

* Điểm các môn (lý thuyết, thực hành...)
* Kết quả từng môn

👉 Hệ thống tự xác định:

```
Đạt / Không đạt
```

👉 Lưu:

* Kết quả chi tiết
* Kết quả tổng hợp


## 📌 6. Cấp giấy phép lái xe

Nếu công dân thi đạt:

* Tạo bản ghi GPLX
* Gán:

  * Hạng GPLX
  * Ngày cấp
  * Trạng thái

👉 Trạng thái mặc định:

```
Còn hiệu lực
```

## 📌 7. Quản lý vi phạm

Sau khi có GPLX:

* Ghi nhận vi phạm
* Trừ điểm GPLX

Thông tin gồm:

* Loại vi phạm
* Mức phạt
* Điểm trừ

👉 Nếu hết điểm:

* Có thể thu hồi GPLX


## 🔄 Mối quan hệ tổng thể

* 1 Công dân → nhiều Hồ sơ
* 1 Hồ sơ → 1 Kỳ thi
* 1 Hồ sơ → nhiều lần thi
* 1 Kỳ thi → nhiều hồ sơ
* 1 GPLX → nhiều vi phạm


## 🎯 Kết luận

Hệ thống tập trung vào:

* Quản lý quy trình cấp GPLX
* Không mô phỏng thi thực tế
* Đảm bảo đơn giản, dễ triển khai nhưng vẫn đầy đủ nghiệp vụ


## Công nghệ sử dụng *
* Font-end: Blazor, HTML, CSS, JS, Boostrap 5
* Back-end: ASP.NET API (.net 8)
* Database: MySQL 

## Cấu trúc thư mục *
```
├QLGPLX/
│
├── Controllers/
│   └── CongDanController.cs
│
├── Models/
│   └── CongDan.cs
│
├── Data/
│   └── GplxDbContext.cs
│
├── DTOs/
│   └── CongDanDTO.cs
│
├── Services/
│   ├── Interfaces/
│   │   └── ICongDanService.cs
|   └── CongDanService.cs
│
├── Repositories/
│   └── CongDanRepository.cs
│
├── Mappings/
│   └── AutoMapperProfile.cs
│
├── appsettings.json
├── Program.cs
```

Chuỗi Scaffold: Scaffold-DbContext "server=localhost;database=;user=;password=;" Pomelo.EntityFrameworkCore.MySql -OutputDir Models -ContextDir Data -Context GplxDbContext -DataAnnotations -NoOnConfiguring -Force


## Các Models liên quan
## Các biến css màu sắc
## File MainLayout
```
QLGPLX_All_Rank
├─ Dockerfile
├─ QLGPLX
│  ├─ QLGPLX
│  │  ├─ .editorconfig
│  │  ├─ appsettings.Development.json
│  │  ├─ Backend.csproj
│  │  ├─ Configurations
│  │  │  └─ CloudinarySettings.cs
│  │  ├─ Controllers
│  │  │  ├─ CongDanController.cs
│  │  │  ├─ HangGiayPhepController.cs
│  │  │  └─ HosoController.cs
│  │  ├─ Data
│  │  │  ├─ gplx.sql
│  │  │  └─ GplxDbContext.cs
│  │  ├─ DTO
│  │  │  ├─ Congdan
│  │  │  │  ├─ CongdanDTO.cs
│  │  │  │  ├─ CreateCongdanDTO.cs
│  │  │  │  └─ UpdateCongdanDTO.cs
│  │  │  ├─ HangGiayPhep
│  │  │  │  └─ HangGiayPhepDTO.cs
│  │  │  └─ HoSo
│  │  │     ├─ CreateHosoDTO.cs
│  │  │     ├─ HosoDTO.cs
│  │  │     └─ UpdateHosoDTO.cs
│  │  ├─ Mapping
│  │  │  └─ AutoMapperProfile.cs
│  │  ├─ Models
│  │  │  ├─ Canbo.cs
│  │  │  ├─ CanboHoso.cs
│  │  │  ├─ Chucvu.cs
│  │  │  ├─ Congdan.cs
│  │  │  ├─ Giayphep.cs
│  │  │  ├─ Hanggiayphep.cs
│  │  │  ├─ HangMonThi.cs
│  │  │  ├─ Hoso.cs
│  │  │  ├─ Ketquachitiet.cs
│  │  │  ├─ Ketquathi.cs
│  │  │  ├─ Kythi.cs
│  │  │  ├─ Lichthi.cs
│  │  │  ├─ Loaivipham.cs
│  │  │  ├─ Monthi.cs
│  │  │  └─ Vipham.cs
│  │  ├─ Program.cs
│  │  ├─ Properties
│  │  │  └─ launchSettings.json
│  │  ├─ QLGPLX.http
│  │  ├─ Repository
│  │  │  ├─ CongdanRepository.cs
│  │  │  ├─ HangGiayPhepRepository.cs
│  │  │  └─ HosoRepository.cs
│  │  ├─ Service
│  │  │  ├─ CloudinaryService.cs
│  │  │  ├─ CongdanService.cs
│  │  │  ├─ HangGiayPhepService.cs
│  │  │  ├─ HosoService .cs
│  │  │  └─ Interface
│  │  │     ├─ ICloudinaryService.cs
│  │  │     ├─ ICongdanService.cs
│  │  │     ├─ IHangGiayPhepService.cs
│  │  │     └─ IHosoService.cs
│  │  └─ wwwroot
│  ├─ QLGPLX.sln
│  └─ UI
│     ├─ Components
│     │  └─ Layout
│     │     ├─ Header.razor
│     │     ├─ MainLayout.razor
│     │     └─ NavMenu.razor
│     └─ wwwroot
│        └─ css
│           └─ app.css
├─ README.md
└─ UI
   ├─ appsettings.Development.json
   ├─ Components
   │  ├─ App.razor
   │  ├─ Layout
   │  │  ├─ Footer.razor
   │  │  ├─ Header.razor
   │  │  ├─ MainLayout.razor
   │  │  └─ NavMenu.razor
   │  ├─ Pages
   │  │  ├─ Congdan
   │  │  │  ├─ DSCongDan.razor
   │  │  │  ├─ SuaCongDan.razor
   │  │  │  └─ ThemCongDan.razor
   │  │  ├─ Error.razor
   │  │  ├─ Home.razor
   │  │  └─ HoSo
   │  │     ├─ Hoso.razor
   │  │     └─ ThemHoSo.razor
   │  ├─ Routes.razor
   │  └─ _Imports.razor
   ├─ DTO
   │  ├─ Congdan
   │  │  ├─ CongdanDTO.cs
   │  │  ├─ CreateCongdanDTO.cs
   │  │  └─ UpdateCongdanDTO.cs
   │  ├─ HangGiayPhep
   │  │  └─ HangGiayPhepDTO.cs
   │  └─ Hoso
   │     ├─ CreateHosoDTO.cs
   │     ├─ HosoDTO.cs
   │     └─ UpdateHosoDTO.cs
   ├─ Program.cs
   ├─ Properties
   │  └─ launchSettings.json
   ├─ UI.csproj
   ├─ Utils
   │  └─ Helper.cs
   └─ wwwroot
      ├─ bootstrap
      │  ├─ bootstrap.min.css
      │  └─ bootstrap.min.css.map
      ├─ css
      │  ├─ app.css
      │  ├─ congdan
      │  │  ├─ ds_congdan.css
      │  │  └─ them_cong_dan.css
      │  └─ hoso
      │     ├─ ds-ho-so.css
      │     └─ them-ho-so.css
      ├─ data
      │  └─ tinh_tp.json
      ├─ favicon.png
      ├─ image
      │  └─ loading.gif
      └─ js
         └─ core
            └─ loading.js

```