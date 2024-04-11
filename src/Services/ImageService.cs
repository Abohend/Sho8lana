using Microsoft.AspNetCore.Hosting;

namespace src.Services
{
	public class ImageService
	{
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ImageService(IWebHostEnvironment webHostEnvironment)
        {
			this._webHostEnvironment = webHostEnvironment;
		}

		#region Helpers
		private string GetImagePath(IFormFile img)
		{
			return $"category/{Guid.NewGuid().ToString()}{Path.GetExtension(img.FileName)}";
		}
		private string GetFilePath(string imagePath)
		{
			return $"{_webHostEnvironment.WebRootPath}/{imagePath}";
		}
		#endregion

		public string UploadImage(IFormFile image)
		{
			var imagePath = GetImagePath(image);
			var filePath = GetFilePath(imagePath);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				image.CopyTo(fileStream);
			}

			return imagePath;
		}
	
		public void DeleteImage(string imgPath)
		{
			var filePath = GetFilePath(imgPath);
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}
	}
}
