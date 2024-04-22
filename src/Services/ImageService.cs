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
		private string GetImageName(IFormFile img)
		{
			return $"{Guid.NewGuid().ToString()}{Path.GetExtension(img.FileName)}";
		}
		private string GetFolderPath(string folderName)
		{
			return Path.Combine(_webHostEnvironment.WebRootPath, folderName);
		}
		#endregion

		public string UploadImage(string folderName, IFormFile image)
		{
			var folderPath = GetFolderPath(folderName);
			var imageName = GetImageName(image);
			var imagePath = Path.Combine(folderPath, imageName);
			
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}

			using (var fileStream = new FileStream(imagePath, FileMode.Create))
			{
				image.CopyTo(fileStream);
			}

			return Path.Combine(folderName, imageName);
		}

		public void DeleteImage(string? imgPath)
		{
			if (imgPath != null)
			{
				var filePath = GetFolderPath(imgPath);
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
				}
			}
		}
	}
}
