using AutoMapper;
using Sho8lana.DataAccess.Data;
using Sho8lana.Entities.Models;
using Sho8lana.Entities.Models.Dto.Category;

namespace Sho8lana.DataAccess.Repositories
{
    public class CategoryRepository
	{
		private readonly Context _db;
		private readonly IMapper _mapper;

		public CategoryRepository(Context db, IMapper mapper)
        {
			_db = db;
			this._mapper = mapper;
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
		public void Create(CreateCategoryDto categoryDto, string ImagePath)
		{
			Category category = _mapper.Map<Category>(categoryDto);
			category.ImagePath = ImagePath;
			_db.Categories.Add(category);
            _db.SaveChanges();
		}
		public void Update(int id, UpdateCategoryDto categoryDto, string? imagePath)
		{
			Category? category = GetCategory(id);
			if (category != null)
			{
				category.Name = categoryDto.Name?? category.Name;
				category.ImagePath = imagePath ?? category.ImagePath;
                _db.SaveChanges();	
			}
		}
		public void Delete(int id)
		{
			var category = GetCategory(id);
			if (category != null)
			{
				_db.Remove(category);
				_db.SaveChanges();
			}
		}
		
	}
}
