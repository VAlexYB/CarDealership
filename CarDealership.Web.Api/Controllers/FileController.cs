using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace CarDealership.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public FileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [Route("upload")]
        [HttpPost]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Файл пуст");
                }

                var uploads = Path.Combine(_environment.ContentRootPath, "uploads");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var newFileName = Path.Combine(uploads, fileName);

                using (var fileStream = new FileStream(newFileName, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return Ok("Файл успешно загружен");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера");
            }
        }

        [Route("getImage/{id}")]
        [HttpGet]
        public IActionResult GetImage(Guid id)
        {
            var uploads = Path.Combine(_environment.ContentRootPath, "uploads");
            var filePath = Path.Combine(uploads, $"{id}.png");
            if (System.IO.File.Exists(filePath))
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return File(fileStream, "image/png");
            }
            else
            {
                return NotFound("Изображение не найдено");
            }
        }
    }
}
