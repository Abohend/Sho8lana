using Microsoft.AspNetCore.Mvc;
using Sho8lana.API.Services;
using RestSharp;

namespace Sho8lana.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CVController : ControllerBase
    {
        private readonly FileService _fileService;
        private readonly IConfiguration _config;

        public CVController(FileService imgService, IConfiguration config)
        {
            this._fileService = imgService;
            this._config = config;
        }

        #region Helpers
        private string? GetImageUrl(string? path)
        {
            if (path != null)
                return $"{Request.Scheme}://{Request.Host}/{path}";
            return null;
        }

        #endregion
        
        // url to consume deployed ai service
        [HttpPost]
        public async Task<IActionResult> PostCV(IFormFile file)
        {
            // define a new client
            var client = new RestClient(_config["AiServices:CvParser:link"]!);

            // define a new post request
            var request = new RestRequest(string.Empty, Method.Post);

            // add headers to the request
            request.AddHeader("apikey", _config["AiServices:CvParser:ApiKey"]!);
            request.AddHeader("Content-Type", "application/octet-stream");

            // send file in body as a binary
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }

            request.AddBody(fileBytes, ContentType.Binary);

            var response = client.Execute(request);

            var responseContent = response.Content;

            return Ok(response.Content);
        }
        
    }
}
