using System.Net;
using System.Net.Mail;
using System.Text;
using Backend.Configurations;
using Backend.Models;
using Backend.Service.Interface;
using Microsoft.Extensions.Options;

namespace Backend.Service;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> options, ILogger<EmailService> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public Task SendHoSoCreatedAsync(Congdan congDan, Hoso hoSo)
    {
        var subject = "Xác nhận đăng ký hồ sơ GPLX thành công";
        var body = BuildLayout(
            "Đăng ký hồ sơ thành công",
            $"Xin chào {Encode(congDan.HoTen)},",
            new[]
            {
                ("Mã hồ sơ", hoSo.HoSoId.ToString()),
                ("Hạng GPLX", FormatHang(hoSo)),
                ("Ngày nộp", FormatDateTime(hoSo.NgayNop)),
                ("Trạng thái", hoSo.TrangThai ?? "Chờ duyệt")
            },
            "Hồ sơ của bạn đã được hệ thống tiếp nhận. Vui lòng theo dõi thông báo tiếp theo từ trung tâm.");

        return SendAsync(congDan.Email, subject, body);
    }

    public Task SendHoSoAddedToKyThiAsync(Congdan congDan, Hoso hoSo, Kythi kyThi)
    {
        var subject = "Thông báo đăng ký kỳ thi GPLX";
        var body = BuildLayout(
            "Đăng ký kỳ thi thành công",
            $"Xin chào {Encode(congDan.HoTen)},",
            new[]
            {
                ("Kỳ thi", kyThi.TenKyThi ?? $"Kỳ thi #{kyThi.KyThiId}"),
                ("Hạng GPLX", FormatHang(hoSo)),
                ("Ngày bắt đầu", FormatDateOnly(kyThi.NgayBatDau)),
                ("Ngày kết thúc", FormatDateOnly(kyThi.NgayKetThuc)),
                ("Địa điểm", kyThi.DiaDiem ?? "Chưa cập nhật")
            },
            "Hồ sơ của bạn đã được thêm vào kỳ thi. Vui lòng có mặt đúng thời gian và địa điểm thông báo.");

        return SendAsync(congDan.Email, subject, body);
    }

    public Task SendKetQuaAsync(Congdan congDan, Hoso hoSo, Kythi? kyThi, Ketquathi ketQua)
    {
        var subject = "Thông báo kết quả thi GPLX";
        var body = BuildLayout(
            "Kết quả thi GPLX",
            $"Xin chào {Encode(congDan.HoTen)},",
            new[]
            {
                ("Kỳ thi", kyThi?.TenKyThi ?? $"Kỳ thi #{ketQua.KyThiId}"),
                ("Hạng GPLX", FormatHang(hoSo)),
                ("Lần thi", (ketQua.LanThi ?? 1).ToString()),
                ("Kết quả tổng hợp", ketQua.KetQuaTongHop ?? "Chưa cập nhật"),
                ("Ngày kết luận", FormatDateTime(ketQua.NgayKetLuan)),
                ("Ghi chú", string.IsNullOrWhiteSpace(ketQua.GhiChu) ? "Không có" : ketQua.GhiChu)
            },
            "Đây là thông báo từ hệ thống quản lý giấy phép lái xe.");

        return SendAsync(congDan.Email, subject, body);
    }

    private async Task SendAsync(string? toEmail, string subject, string htmlBody)
    {
        if (string.IsNullOrWhiteSpace(toEmail))
            return;

        if (string.IsNullOrWhiteSpace(_settings.SmtpHost) ||
            string.IsNullOrWhiteSpace(_settings.FromPassword) ||
            string.IsNullOrWhiteSpace(_settings.FromEmail))
        {
            _logger.LogWarning("Cấu hình email chưa đầy đủ. Bỏ qua gửi email đến {Email}", toEmail);
            return;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(_settings.FromEmail, "Hệ thống QLGPLX", Encoding.UTF8),
            Subject = subject,
            SubjectEncoding = Encoding.UTF8,
            Body = htmlBody,
            BodyEncoding = Encoding.UTF8,
            IsBodyHtml = true
        };

        message.To.Add(toEmail);

        using var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_settings.FromEmail, _settings.FromPassword)
        };

        await client.SendMailAsync(message);
    }

    private static string BuildLayout(
        string title,
        string greeting,
        IEnumerable<(string Label, string Value)> rows,
        string footer)
    {
        var rowHtml = string.Join("", rows.Select(row =>
            $"""
            <tr>
                <td style="padding:10px 12px;border-bottom:1px solid #e5e7eb;color:#475569;width:160px">{Encode(row.Label)}</td>
                <td style="padding:10px 12px;border-bottom:1px solid #e5e7eb;color:#0f172a;font-weight:600">{Encode(row.Value)}</td>
            </tr>
            """));

        return $"""
        <div style="font-family:Arial,sans-serif;background:#f8fafc;padding:24px;color:#0f172a">
            <div style="max-width:640px;margin:0 auto;background:#ffffff;border:1px solid #e5e7eb;border-radius:8px;overflow:hidden">
                <div style="background:#0f766e;color:#ffffff;padding:18px 22px">
                    <h2 style="margin:0;font-size:20px">{Encode(title)}</h2>
                </div>
                <div style="padding:22px">
                    <p style="margin:0 0 16px">{greeting}</p>
                    <table style="border-collapse:collapse;width:100%;border:1px solid #e5e7eb">{rowHtml}</table>
                    <p style="margin:18px 0 0;color:#475569">{Encode(footer)}</p>
                </div>
            </div>
        </div>
        """;
    }

    private static string FormatHang(Hoso hoSo)
    {
        var tenHang = hoSo.MaHangNavigation?.TenHang;
        return string.IsNullOrWhiteSpace(tenHang) ? hoSo.MaHang : $"{hoSo.MaHang} - {tenHang}";
    }

    private static string FormatDateTime(DateTime? value)
    {
        return value?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa cập nhật";
    }

    private static string FormatDateOnly(DateOnly? value)
    {
        return value?.ToString("dd/MM/yyyy") ?? "Chưa cập nhật";
    }

    private static string Encode(string? value)
    {
        return WebUtility.HtmlEncode(value ?? string.Empty);
    }
}
