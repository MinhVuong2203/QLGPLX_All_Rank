using Backend.Service.Interface;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
namespace Backend.Service;
public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }
    public async Task<string?> UploadImageAsync(IFormFile? file, string folder, string publicId)
    {
        if (file == null) return null;

        var ext = Path.GetExtension(file.FileName).ToLower();
        string[] allowed = [".jpg", ".jpeg", ".png"];

        if (!allowed.Contains(ext))
            throw new Exception("File phải là jpg, jpeg hoặc png");

        // 👉 Tạo tên dễ quản lý
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var fileName = $"{publicId}_{timestamp}";

        using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = folder,
            PublicId = fileName,
            Overwrite = true // cho phép ghi đè nếu trùng
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error != null || result.SecureUrl == null)
            throw new Exception(result.Error?.Message ?? "Upload thất bại");

        return result.SecureUrl.ToString();
    }
}