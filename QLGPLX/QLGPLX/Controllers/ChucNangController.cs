using Backend.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChucNangController : ControllerBase
    {
        private readonly IChucNangService _chucNangService;

        public ChucNangController(IChucNangService chucNangService)
        {
            _chucNangService = chucNangService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _chucNangService.GetAllAsync();
            return Ok(result);
        }
    }
}
