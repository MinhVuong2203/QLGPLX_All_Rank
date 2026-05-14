// Service/Implement/KetQuaService.cs

using Backend.DTO.KetQua;
using Backend.Repository;
using Backend.Service.Interface;
using Backend.Models;

namespace Backend.Service.Implement
{
    public class KetQuaService : IKetQuaService
    {
        private readonly KetQuaRepository _ketQuaRepo;

        public KetQuaService(KetQuaRepository ketQuaRepo)
        {
            _ketQuaRepo = ketQuaRepo;
        }

        public async Task<List<HoSoKetQuaDTO>> GetHoSoKetQuaByKyThiAsync(int kyThiId)
        {
            var hoSoList = await _ketQuaRepo.GetHoSoByKyThiAsync(kyThiId);
            var result = new List<HoSoKetQuaDTO>();

            foreach (var hoSo in hoSoList)
            {
                var ketQuaList =
                    await _ketQuaRepo.GetKetQuaByHoSoAndKyThiAsync(
                        hoSo.HoSoId,
                        kyThiId);

                var ketQuaDTOList = new List<KetQuaThiDTO>();

                var monThiList =
                    await _ketQuaRepo.GetMonThiByHangAsync(
                        hoSo.MaHang);

                foreach (var ketQua in ketQuaList)
                {
                    var chiTietList =
                        await _ketQuaRepo
                            .GetKetQuaChiTietByKetQuaIdAsync(
                                ketQua.KetQuaId);

                    var chiTietDTOList =
                        chiTietList.Select(c =>
                        {
                            var monThiInfo =
                                monThiList.FirstOrDefault(
                                    m => m.MonThiid == c.MonThiId);

                            return new KetQuaChiTietDTO
                            {
                                ChiTietID = c.ChiTietId,
                                KetQuaID = (int)c.KetQuaId,
                                MonThiID = (int)c.MonThiId,
                                TenMon = c.MonThi.TenMon,
                                Diem = c.Diem,
                                DiemDat = monThiInfo?.DiemDat ?? 0,
                                DiemToiDa = monThiInfo?.DiemToiDa ?? 100,
                                ThoiGianBatDau = c.ThoiGianBatDau,
                                KetQua = c.KetQua,
                                GhiChu = c.GhiChu
                            };
                        }).ToList();

                    ketQuaDTOList.Add(new KetQuaThiDTO
                    {
                        KetQuaID = ketQua.KetQuaId,
                        HoSoID = (int)ketQua.HoSoId,
                        KyThiID = (int)ketQua.KyThiId,
                        KetQuaTongHop = ketQua.KetQuaTongHop,
                        NgayKetLuan = ketQua.NgayKetLuan ?? DateTime.Now,
                        LanThi = ketQua.LanThi ?? 1,
                        GhiChu = ketQua.GhiChu,
                        ChiTiet = chiTietDTOList
                    });
                }

                result.Add(new HoSoKetQuaDTO
                {
                    HoSoID = hoSo.HoSoId,
                    PublicId = (Guid)hoSo.PublicId,
                    HoTen = hoSo.MaCongDanNavigation.HoTen,
                    CCCD = hoSo.MaCongDanNavigation.Cccd,
                    NgaySinh = hoSo.MaCongDanNavigation.NgaySinh,
                    MaHang = hoSo.MaHang,
                    TenHang = hoSo.MaHangNavigation.TenHang,
                    KetQuaThiList = ketQuaDTOList
                });
            }

            return result.OrderBy(r => r.HoTen).ToList();
        }

        public async Task<KetQuaThiDTO> GetKetQuaByIdAsync(int ketQuaId)
        {
            var ketQua = await _ketQuaRepo.GetKetQuaByIdAsync(ketQuaId);

            if (ketQua == null)
                return null;

            var chiTietList =
                await _ketQuaRepo.GetKetQuaChiTietByKetQuaIdAsync(
                    ketQuaId);

            var hoSo =
                await _ketQuaRepo.GetHoSoByKyThiAsync(
                    (int)ketQua.KyThiId);

            var currentHoSo =
                hoSo.FirstOrDefault(
                    h => h.HoSoId == ketQua.HoSoId);

            var monThiList =
                await _ketQuaRepo.GetMonThiByHangAsync(
                    currentHoSo?.MaHang ?? "");

            var chiTietDTOList =
                chiTietList.Select(c =>
                {
                    var monThiInfo =
                        monThiList.FirstOrDefault(
                            m => m.MonThiid == c.MonThiId);

                    return new KetQuaChiTietDTO
                    {
                        ChiTietID = c.ChiTietId,
                        KetQuaID = c.KetQuaId,
                        MonThiID = c.MonThiId,
                        TenMon = c.MonThi.TenMon,
                        Diem = c.Diem,
                        DiemDat = monThiInfo?.DiemDat ?? 0,
                        DiemToiDa = monThiInfo?.DiemToiDa ?? 100,
                        ThoiGianBatDau = c.ThoiGianBatDau,
                        KetQua = c.KetQua,
                        GhiChu = c.GhiChu
                    };
                }).ToList();

            return new KetQuaThiDTO
            {
                KetQuaID = ketQua.KetQuaId,
                HoSoID = (int)ketQua.HoSoId,
                KyThiID = (int)ketQua.KyThiId,
                KetQuaTongHop = ketQua.KetQuaTongHop,
                NgayKetLuan = ketQua.NgayKetLuan ?? DateTime.Now,
                LanThi = ketQua.LanThi ?? 1,
                GhiChu = ketQua.GhiChu,
                ChiTiet = chiTietDTOList
            };
        }

        // =========================================================
        // CREATE
        // =========================================================

        public async Task<KetQuaThiDTO> CreateKetQuaAsync(
            CreateKetQuaDTO dto)
        {
            if (dto.LanThi > 3)
                throw new ArgumentException(
                    "Số lần thi không được vượt quá 3");

            var ketQua = new Ketquathi
            {
                HoSoId = dto.HoSoID,
                KyThiId = dto.KyThiID,
                LanThi = dto.LanThi,
                NgayKetLuan = DateTime.Now,
                GhiChu = dto.GhiChu
            };

            await _ketQuaRepo.CreateKetQuaAsync(ketQua);

            var allPassed = true;

            foreach (var chiTiet in dto.ChiTiet)
            {
                if (chiTiet.KetQua != "Đạt")
                    allPassed = false;

                var ketQuaChiTiet = new Ketquachitiet
                {
                    KetQuaId = ketQua.KetQuaId,
                    MonThiId = chiTiet.MonThiID,
                    Diem = chiTiet.Diem,
                    ThoiGianBatDau =
                        chiTiet.ThoiGianBatDau ?? DateTime.Now,

                    // FRONTEND quyết định
                    KetQua = chiTiet.KetQua,

                    GhiChu = chiTiet.GhiChu
                };

                await _ketQuaRepo.CreateKetQuaChiTietAsync(
                    ketQuaChiTiet);
            }

            ketQua.KetQuaTongHop =
                allPassed ? "Đạt" : "Không đạt";

            await _ketQuaRepo.UpdateKetQuaAsync(
                ketQua);

            await _ketQuaRepo.SaveChangesAsync();

            return await GetKetQuaByIdAsync(
                ketQua.KetQuaId);
        }

        // =========================================================
        // UPDATE
        // =========================================================

        public async Task<KetQuaThiDTO> UpdateKetQuaAsync(
            int ketQuaId,
            UpdateKetQuaDTO dto)
        {
            var ketQua =
                await _ketQuaRepo.GetKetQuaByIdAsync(
                    ketQuaId);

            if (ketQua == null)
                throw new ArgumentException(
                    "Không tìm thấy kết quả thi");

            var allPassed = true;

            foreach (var chiTiet in dto.ChiTiet)
            {
                var existing =
                    await _ketQuaRepo
                        .GetKetQuaChiTietByKetQuaIdAsync(
                            ketQuaId);

                var current =
                    existing.FirstOrDefault(
                        c => c.ChiTietId == chiTiet.ChiTietID);

                if (current != null)
                {
                    if (chiTiet.KetQua != "Đạt")
                        allPassed = false;

                    current.Diem = chiTiet.Diem;
                    current.ThoiGianBatDau =
                        chiTiet.ThoiGianBatDau;

                    // FRONTEND quyết định
                    current.KetQua = chiTiet.KetQua;

                    current.GhiChu = chiTiet.GhiChu;

                    await _ketQuaRepo
                        .UpdateKetQuaChiTietAsync(
                            current);
                }
            }

            ketQua.KetQuaTongHop =
                allPassed ? "Đạt" : "Không đạt";

            ketQua.GhiChu = dto.GhiChu;
            ketQua.NgayKetLuan = DateTime.Now;

            await _ketQuaRepo.UpdateKetQuaAsync(
                ketQua);

            await _ketQuaRepo.SaveChangesAsync();

            return await GetKetQuaByIdAsync(
                ketQuaId);
        }

        // =========================================================
        // DELETE
        // =========================================================

        public async Task<bool> DeleteKetQuaAsync(
            int ketQuaId)
        {
            var ketQua =
                await _ketQuaRepo.GetKetQuaByIdAsync(
                    ketQuaId);

            if (ketQua == null)
                return false;

            await _ketQuaRepo.DeleteKetQuaAsync(
                ketQua);

            await _ketQuaRepo.SaveChangesAsync();

            return true;
        }

        // =========================================================
        // UPDATE TỔNG HỢP
        // =========================================================

        private async Task UpdateKetQuaTongHopAsync(
            int ketQuaId)
        {
            var ketQua =
                await _ketQuaRepo.GetKetQuaByIdAsync(
                    ketQuaId);

            if (ketQua == null)
                return;

            var chiTietList =
                await _ketQuaRepo
                    .GetKetQuaChiTietByKetQuaIdAsync(
                        ketQuaId);

            var allPassed =
                chiTietList.All(
                    c => c.KetQua == "Đạt");

            ketQua.KetQuaTongHop =
                allPassed ? "Đạt" : "Không đạt";

            ketQua.NgayKetLuan = DateTime.Now;

            await _ketQuaRepo.UpdateKetQuaAsync(
                ketQua);

            await _ketQuaRepo.SaveChangesAsync();
        }


        // =========================================================
        // CREATE CHI TIẾT
        // =========================================================

        public async Task<KetQuaChiTietDTO>
            CreateKetQuaChiTietAsync(
                int ketQuaId,
                CreateKetQuaChiTietDTO dto)
        {
            var ketQua =
                await _ketQuaRepo.GetKetQuaByIdAsync(
                    ketQuaId);

            if (ketQua == null)
                throw new ArgumentException(
                    "Không tìm thấy kết quả thi");

            var hoSo =
                await _ketQuaRepo.GetHoSoByIdAsync(
                    ketQua.HoSoId);

            var monThiList =
                await _ketQuaRepo.GetMonThiByHangAsync(
                    hoSo.MaHang);

            var monThiInfo =
                monThiList.FirstOrDefault(
                    m => m.MonThiid == dto.MonThiID);

            if (monThiInfo == null)
                throw new ArgumentException(
                    "Môn thi không hợp lệ cho hạng này");

            var chiTiet = new Ketquachitiet
            {
                KetQuaId = ketQuaId,
                MonThiId = dto.MonThiID,
                Diem = dto.Diem,

                ThoiGianBatDau =
                    dto.ThoiGianBatDau ?? DateTime.Now,

                // FRONTEND quyết định
                KetQua = dto.KetQua,

                GhiChu = dto.GhiChu
            };

            await _ketQuaRepo
                .CreateKetQuaChiTietAsync(
                    chiTiet);

            await _ketQuaRepo.SaveChangesAsync();

            await UpdateKetQuaTongHopAsync(
                ketQuaId);

            return new KetQuaChiTietDTO
            {
                ChiTietID = chiTiet.ChiTietId,
                KetQuaID = chiTiet.KetQuaId,
                MonThiID = chiTiet.MonThiId,
                TenMon = monThiInfo.MonThi.TenMon,
                Diem = chiTiet.Diem,
                DiemDat = monThiInfo.DiemDat,
                DiemToiDa = monThiInfo.DiemToiDa,
                ThoiGianBatDau = chiTiet.ThoiGianBatDau,
                KetQua = chiTiet.KetQua,
                GhiChu = chiTiet.GhiChu
            };
        }

        // =========================================================
        // UPDATE CHI TIẾT
        // =========================================================

        public async Task<KetQuaChiTietDTO>
            UpdateKetQuaChiTietAsync(
                int chiTietId,
                UpdateKetQuaChiTietDTO dto)
        {
            var chiTiet =
                await _ketQuaRepo
                    .GetKetQuaChiTietByIdAsync(
                        chiTietId);

            if (chiTiet == null)
                throw new ArgumentException(
                    "Không tìm thấy kết quả chi tiết");

            var ketQua =
                await _ketQuaRepo.GetKetQuaByIdAsync(
                    chiTiet.KetQuaId);

            var hoSo =
                await _ketQuaRepo.GetHoSoByIdAsync(
                    ketQua.HoSoId);

            var monThiList =
                await _ketQuaRepo.GetMonThiByHangAsync(
                    hoSo.MaHang);

            var monThiInfo =
                monThiList.FirstOrDefault(
                    m => m.MonThiid == chiTiet.MonThiId);

            chiTiet.Diem = dto.Diem;

            chiTiet.ThoiGianBatDau =
                dto.ThoiGianBatDau;

            // FRONTEND quyết định
            chiTiet.KetQua = dto.KetQua;

            chiTiet.GhiChu = dto.GhiChu;

            await _ketQuaRepo
                .UpdateKetQuaChiTietAsync(
                    chiTiet);

            await _ketQuaRepo.SaveChangesAsync();

            await UpdateKetQuaTongHopAsync(
                chiTiet.KetQuaId);

            return new KetQuaChiTietDTO
            {
                ChiTietID = chiTiet.ChiTietId,
                KetQuaID = chiTiet.KetQuaId,
                MonThiID = chiTiet.MonThiId,
                TenMon = chiTiet.MonThi.TenMon,
                Diem = chiTiet.Diem,
                DiemDat = monThiInfo?.DiemDat ?? 0,
                DiemToiDa = monThiInfo?.DiemToiDa ?? 100,
                ThoiGianBatDau = chiTiet.ThoiGianBatDau,
                KetQua = chiTiet.KetQua,
                GhiChu = chiTiet.GhiChu
            };
        }

        // =========================================================
        // DELETE CHI TIẾT
        // =========================================================

        public async Task<bool>
            DeleteKetQuaChiTietAsync(
                int chiTietId)
        {
            var chiTiet =
                await _ketQuaRepo
                    .GetKetQuaChiTietByIdAsync(
                        chiTietId);

            if (chiTiet == null)
                return false;

            var ketQuaId = chiTiet.KetQuaId;

            _ketQuaRepo.DeleteKetQuaChiTietAsync(
                chiTiet);

            await _ketQuaRepo.SaveChangesAsync();

            await UpdateKetQuaTongHopAsync(
                ketQuaId);

            return true;
        }

        // =========================================================
        // GET CHI TIẾT
        // =========================================================

        public async Task<List<KetQuaChiTietDTO>>
            GetKetQuaChiTietByKetQuaIdAsync(
                int ketQuaId)
        {
            var chiTietList =
                await _ketQuaRepo
                    .GetKetQuaChiTietByKetQuaIdAsync(
                        ketQuaId);

            var ketQua =
                await _ketQuaRepo.GetKetQuaByIdAsync(
                    ketQuaId);

            var hoSo =
                await _ketQuaRepo.GetHoSoByIdAsync(
                    ketQua.HoSoId);

            var monThiList =
                await _ketQuaRepo.GetMonThiByHangAsync(
                    hoSo.MaHang);

            return chiTietList.Select(c =>
            {
                var monThiInfo =
                    monThiList.FirstOrDefault(
                        m => m.MonThiid == c.MonThiId);

                return new KetQuaChiTietDTO
                {
                    ChiTietID = c.ChiTietId,
                    KetQuaID = c.KetQuaId,
                    MonThiID = c.MonThiId,
                    TenMon = c.MonThi.TenMon,
                    Diem = c.Diem,
                    DiemDat = monThiInfo?.DiemDat ?? 0,
                    DiemToiDa = monThiInfo?.DiemToiDa ?? 100,
                    ThoiGianBatDau = c.ThoiGianBatDau,
                    KetQua = c.KetQua,
                    GhiChu = c.GhiChu
                };
            }).ToList();
        }
    }
}