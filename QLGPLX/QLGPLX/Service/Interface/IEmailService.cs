using Backend.Models;

namespace Backend.Service.Interface;

public interface IEmailService
{
    Task SendHoSoCreatedAsync(Congdan congDan, Hoso hoSo);
    Task SendHoSoAddedToKyThiAsync(Congdan congDan, Hoso hoSo, Kythi kyThi);
    Task SendKetQuaAsync(Congdan congDan, Hoso hoSo, Kythi? kyThi, Ketquathi ketQua);
}
