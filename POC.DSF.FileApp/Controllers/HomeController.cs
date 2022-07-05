using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using POC.DSF.FileApp.Models;
using POC.DSF.FileApp.Settings;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace POC.DSF.FileApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;
        public HomeController(IOptions<AppSettings> options, ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _appSettings = options.Value;
            _httpClient = httpClientFactory.CreateClient();
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("{fileName}")]
        public async Task<FileResult> GetFile(string fileName)
        {
            try
            {
                var response = await _httpClient.GetAsync(new Uri($"{_appSettings.FileStorageApiUrl}/api/FileStorage/{fileName}"));
                return File(response.Content.ReadAsStream(), response.Content.Headers.ContentType.MediaType);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Submit(FileModel fileModel)
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StreamContent(fileModel.File.OpenReadStream())
                {
                    Headers =
                    {
                        ContentLength = fileModel.File.Length,
                        ContentType = new MediaTypeHeaderValue(fileModel.File.ContentType)
                    }
                }, "File", fileModel.File.FileName);

                var response = await _httpClient.PostAsync(new Uri($"{_appSettings.FileStorageApiUrl}/api/FileStorage"), content);
            }
            return RedirectToAction(nameof(Index));

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