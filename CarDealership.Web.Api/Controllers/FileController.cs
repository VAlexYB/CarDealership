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
                if (file.Length > 0)
                {
                    var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    var filePath = Path.Combine(uploads, ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    return Ok("Файл успешно загружен");
                }
                else
                {
                    return BadRequest("Файл пуст");
                }
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
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, id + ".png");
            if (System.IO.File.Exists(filePath))
            {
                var fileStream = new FileStream(filePath, FileMode.Open);
                return File(fileStream, "image/png");
            }
            else
            {
                return NotFound("Изображение не найдено");
            }
        }
    }
}
