using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using src.Data;
using src.Models;
using src.Models.Dto.Category;
using src.Services;
using static System.Net.Mime.MediaTypeNames;

namespace src.Repository
{
    public class CategoryRepository
	{
		private readonly Context _db;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly ImageService _imageService;

		public CategoryRepository(Context db, IMapper mapper, IWebHostEnvironment webHostEnvironment
			,ImageService imageService)
        {
			_db = db;
			this._mapper = mapper;
			this._webHostEnvironment = webHostEnvironment;
			this._imageService = imageService;
		}

		#region Helpers
		private Category? GetCategory(int id)
		{
			return _db.Categories.Find(id);
		}

		#endregion

		public List<Category>? GetAll()
		{
			return  _db.Categories.ToList();
		}

		public Category? Get(int id)
		{
			return _db.Categories.Find(id);
		}
		public void Create(CreateCategoryDto categoryDto)
		{
			Category category = _mapper.Map<Category>(categoryDto);
			if (categoryDto.Image != null)
				category.ImagePath = _imageService.UploadImage(categoryDto.Image);
			_db.Add(category);
			_db.SaveChanges();
		}
		public void Update(int id, UpdateCategoryDto categoryDto)
		{
			Category? category = GetCategory(id);
			if (category != null)
			{
				if (categoryDto.Image != null)
				{
					_imageService.DeleteImage(category.ImagePath);
					category.ImagePath = _imageService.UploadImage(categoryDto.Image);
				}
				category.Name = categoryDto.Name;
				_db.SaveChanges();	
			}
		}
		public void Delete(int id)
		{
			var category = GetCategory(id);
			if (category != null)
			{
				_db.Remove(category);
				_imageService.DeleteImage(category.ImagePath);
				_db.SaveChanges();
			}
		}
		
	}
}
