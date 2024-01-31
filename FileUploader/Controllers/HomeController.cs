using FileUploader.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FileUploader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            string[] filePaths = Directory.GetFiles(path);

            List<UploadedFile> files = new List<UploadedFile>();
            foreach (string filePath in filePaths)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                files.Add(new UploadedFile()
                {
                    FileName = fileInfo.Name,
                    FileSize = $"{fileInfo.Length / 1024 / 1024} MB",
                    FileLink = $"http://localhost:5000/uploads/{fileInfo.Name}"
                });

                //files.Add($"{fileInfo.Name} | {fileInfo.Length / 1024 / 1024} MB");
            }

            return View(files);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return Ok();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
