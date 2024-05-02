using Microsoft.AspNetCore.Mvc;

namespace ViL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadsController : Controller
    {
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                var apiKey = "4dc088e5e644f3290818694864da17d0";
                var apiUrl = $"https://api.imgbb.com/1/upload?key={apiKey}";
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Invalid file to upload!");
                }
                using (HttpClient client = new HttpClient())
                {
                    using (var uploadStream = new MemoryStream())
                    {
                        await file.CopyToAsync(uploadStream);
                        var formData = new MultipartFormDataContent();

                        formData.Add(new StringContent(apiKey), "key");
                        formData.Add(new ByteArrayContent(uploadStream.ToArray()), "image", file.FileName);


                        var response = await client.PostAsync(apiUrl, formData);

                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            return Ok(result);
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
