using Backend.Data;
using Backend.DTO.CanBo;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

public class CanBoRepository
{
    private readonly GplxDbContext _context;

    public CanBoRepository(GplxDbContext context)
    {
        _context = context;
    }

    public async Task<List<CanBoResponseDto>> GetAllAsync(string? keyword, bool? trangThai)
    {
        var query = _context.Canbos
            .Include(cb => cb.MaChucVuNavigation)
            .Include(cb => cb.MaChucNangs)
            .Where(cb => cb.MaChucVuNavigation == null || cb.MaChucVuNavigation.TenChucVu != "Quản lý")
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();

            query = query.Where(cb =>
                (cb.HoTen != null && cb.HoTen.Contains(keyword)) ||
                cb.Email.Contains(keyword) ||
                cb.Cccd.Contains(keyword) ||
                cb.Username.Contains(keyword)
            );
        }

        if (trangThai.HasValue)
        {
            query = query.Where(cb => cb.TrangThai == trangThai.Value);
        }

        return await query
            .OrderByDescending(cb => cb.NgayTao)
            .Select(cb => new CanBoResponseDto
            {
                MaCanBo = cb.MaCanBo,
                PublicId = (Guid)cb.PublicId,
                HoTen = cb.HoTen,
                MaChucVu = cb.MaChucVu,
                TenChucVu = cb.MaChucVuNavigation != null
                    ? cb.MaChucVuNavigation.TenChucVu
                    : null,
                Email = cb.Email,
                Cccd = cb.Cccd,
                DienThoai = cb.DienThoai,
                NgayTao = cb.NgayTao,
                Anh3x4 = cb.Anh3x4,
                Username = cb.Username,
                TrangThai = cb.TrangThai ?? false,
                SoQuyen = cb.MaChucNangs.Count
            })
            .ToListAsync();
    }

    public async Task<CanBoResponseDto?> GetByPublicIdAsync(Guid publicId)
    {
        return await _context.Canbos
            .Include(cb => cb.MaChucVuNavigation)
            .Include(cb => cb.MaChucNangs)
            .Where(cb => cb.PublicId == publicId)
            .Select(cb => new CanBoResponseDto
            {
                MaCanBo = cb.MaCanBo,
                PublicId = (Guid)cb.PublicId,
                HoTen = cb.HoTen,
                MaChucVu = cb.MaChucVu,
                TenChucVu = cb.MaChucVuNavigation != null
                    ? cb.MaChucVuNavigation.TenChucVu
                    : null,
                Email = cb.Email,
                Cccd = cb.Cccd,
                DienThoai = cb.DienThoai,
                NgayTao = cb.NgayTao,
                Anh3x4 = cb.Anh3x4,
                Username = cb.Username,
                TrangThai = cb.TrangThai ?? false,
                SoQuyen = cb.MaChucNangs.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Canbo?> GetEntityByPublicIdAsync(Guid publicId)
    {
        return await _context.Canbos
            .FirstOrDefaultAsync(cb => cb.PublicId == publicId);
    }

    public async Task AddAsync(Canbo canBo)
    {
        _context.Canbos.Add(canBo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Canbo canBo)
    {
        _context.Canbos.Update(canBo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Canbo canBo)
    {
        _context.Canbos.Remove(canBo);
        await _context.SaveChangesAsync();
    }

    public async Task ChangeStatusAsync(Canbo canBo, bool trangThai)
    {
        canBo.TrangThai = trangThai;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Chucvu>> GetChucVuAsync()
    {
        return await _context.Chucvus
            .OrderBy(cv => cv.MaChucVu)
            .ToListAsync();
    }

    public async Task<List<int>> GetQuyenByCanBoAsync(Guid publicId)
    {
        var canBo = await _context.Canbos
            .Include(cb => cb.MaChucNangs)
            .FirstOrDefaultAsync(cb => cb.PublicId == publicId);

        if (canBo == null)
        {
            return new List<int>();
        }

        return canBo.MaChucNangs
            .Select(cn => cn.MaChucNang)
            .ToList();
    }

    public async Task UpdateQuyenAsync(Guid publicId, List<int> maChucNangs)
    {
        var canBo = await _context.Canbos
            .Include(cb => cb.MaChucNangs)
            .FirstOrDefaultAsync(cb => cb.PublicId == publicId);

        if (canBo == null)
        {
            throw new Exception("Không tìm thấy cán bộ");
        }

        var danhSachQuyenMoi = await _context.Chucnangs
            .Where(cn => maChucNangs.Contains(cn.MaChucNang))
            .ToListAsync();

        canBo.MaChucNangs.Clear();

        foreach (var chucNang in danhSachQuyenMoi)
        {
            canBo.MaChucNangs.Add(chucNang);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByPublicIdAsync(Guid publicId)
    {
        return await _context.Canbos
            .AnyAsync(cb => cb.PublicId == publicId);
    }

    public async Task<bool> ExistsEmailAsync(string email, Guid? ignorePublicId = null)
    {
        var query = _context.Canbos
            .Where(cb => cb.Email == email);

        if (ignorePublicId.HasValue)
        {
            query = query.Where(cb => cb.PublicId != ignorePublicId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<bool> ExistsCccdAsync(string cccd, Guid? ignorePublicId = null)
    {
        var query = _context.Canbos
            .Where(cb => cb.Cccd == cccd);

        if (ignorePublicId.HasValue)
        {
            query = query.Where(cb => cb.PublicId != ignorePublicId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<bool> ExistsUsernameAsync(string username, Guid? ignorePublicId = null)
    {
        var query = _context.Canbos
            .Where(cb => cb.Username == username);

        if (ignorePublicId.HasValue)
        {
            query = query.Where(cb => cb.PublicId != ignorePublicId.Value);
        }

        return await query.AnyAsync();
    }
}