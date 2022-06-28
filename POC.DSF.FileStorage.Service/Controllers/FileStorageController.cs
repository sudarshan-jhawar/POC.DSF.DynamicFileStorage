using Microsoft.AspNetCore.Mvc;
using POC.DSF.FileStorage.Service.Abstractions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace POC.DSF.FileStorage.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileStorageController : ControllerBase
    {
        private readonly IFileStorageService fileStorageService;

        public FileStorageController(IFileStorageService fileStorageService)
        {
            this.fileStorageService = fileStorageService;
        }
        // GET: api/<FileStorageController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<FileStorageController>/5
        [HttpGet("{fileName}")]
        public async Task<FileResult> Get(string fileName)
        {
            try
            {
                (Stream content, string type) = await fileStorageService.DownloadAsync(fileName);
                return File(content, type, fileName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST api/<FileStorageController>
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            try
            {
                using MemoryStream memoryStream = new();
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                await fileStorageService.UploadAsync(memoryStream,file.FileName,file.ContentType);
                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //// PUT api/<FileStorageController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<FileStorageController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
