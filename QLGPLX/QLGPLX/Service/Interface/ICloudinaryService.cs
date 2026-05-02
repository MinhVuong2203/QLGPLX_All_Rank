namespace Backend.Service.Interface;
public interface ICloudinaryService
{
    Task<string?> UploadImageAsync(IFormFile? file, string folder, string publicId);
}
