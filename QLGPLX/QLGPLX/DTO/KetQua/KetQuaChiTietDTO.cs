namespace Backend.DTO.KetQua
{
    public class KetQuaChiTietDTO
    {
        public int ChiTietID { get; set; }
        public int KetQuaID { get; set; }
        public int MonThiID { get; set; }
        public string TenMon { get; set; }
        public int Diem { get; set; }
        public int DiemDat { get; set; }
        public int DiemToiDa { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public string KetQua { get; set; }
        public string GhiChu { get; set; }
    }

    public class KetQuaThiDTO
    {
        public int KetQuaID { get; set; }
        public int HoSoID { get; set; }
        public int KyThiID { get; set; }
        public string KetQuaTongHop { get; set; }
        public DateTime NgayKetLuan { get; set; }
        public int LanThi { get; set; }
        public string GhiChu { get; set; }
        public List<KetQuaChiTietDTO> ChiTiet { get; set; }
    }

    public class HoSoKetQuaDTO
    {
        public int HoSoID { get; set; }
        public Guid PublicId { get; set; }
        public string HoTen { get; set; }
        public string CCCD { get; set; }
        public DateOnly NgaySinh { get; set; }
        public string MaHang { get; set; }
        public string TenHang { get; set; }
        public List<KetQuaThiDTO> KetQuaThiList { get; set; }
    }

    public class CreateKetQuaDTO
    {
        public int HoSoID { get; set; }
        public int KyThiID { get; set; }
        public int LanThi { get; set; }
        public List<CreateKetQuaChiTietDTO> ChiTiet { get; set; }
        public string GhiChu { get; set; }
    }

    public class CreateKetQuaChiTietDTO
    {
        public int MonThiID { get; set; }
        public int Diem { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public string KetQua { get; set; }
        public string GhiChu { get; set; }
    }

    public class UpdateKetQuaDTO
    {
        public List<UpdateKetQuaChiTietDTO> ChiTiet { get; set; }
        public string GhiChu { get; set; }
    }

    public class UpdateKetQuaChiTietDTO
    {
        public int ChiTietID { get; set; }
        public int Diem { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public string KetQua { get; set; }
        public string GhiChu { get; set; }
    }
}