using CarDealership.Web.Api.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http.Headers;

namespace CarDealership.Tests.ControllersTests
{
    public class FileControllerTests
    {
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;
        private readonly Mock<ILogger<FileController>> _mockLogger;
        private readonly FileController _controller;
        private readonly string _uploads;

        public FileControllerTests()
        {
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _mockLogger = new Mock<ILogger<FileController>>();
            _controller = new FileController(_mockEnvironment.Object, _mockLogger.Object);

            _uploads = Path.Combine(AppContext.BaseDirectory, "uploads");
            if (!Directory.Exists(_uploads))
            {
                Directory.CreateDirectory(_uploads);
            }
            _mockEnvironment.Setup(env => env.ContentRootPath).Returns(AppContext.BaseDirectory);
        }


        [Fact]
        public void Upload_ReturnsOk_WhenFileIsUploaded()
        {
            // Arrange
            var fileName = "testfile.txt";
            var filePath = Path.Combine(_uploads, fileName);
            var fileContent = "Test file content";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(fileContent);
            writer.Flush();
            stream.Position = 0;

            var formFile = new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentDisposition = $"form-data; name=\"file\"; filename=\"{fileName}\""
            };

            var formFileCollection = new FormFileCollection { formFile };
            var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(), formFileCollection);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { Request = { Form = formCollection } }
            };

            // Act
            var result = _controller.Upload();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Файл успешно загружен", okResult.Value);
            Assert.True(System.IO.File.Exists(filePath));

            // Cleanup
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        [Fact]
        public void GetImage_ReturnsNotFound_WhenFileDoesNotExist()
        {
            // Arrange
            var fileId = Guid.NewGuid();

            // Act
            var result = _controller.GetImage(fileId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Изображение не найдено", notFoundResult.Value);
        }
    }
}
