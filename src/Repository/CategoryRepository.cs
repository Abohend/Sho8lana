using AutoMapper;
using Microsoft.Identity.Client;
using src.Data;
using src.Models;
using src.Models.Dto;

namespace src.Repository
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

		public List<CategoryDto>? GetAll()
		{
			return _mapper.Map<List<CategoryDto>>(_db.Categories.ToList());
		}

		public CategoryDto? Get(int id)
		{
			return _mapper.Map<CategoryDto>(_db.Categories.Find(id));
		}

		public void Create(CategoryDto categoryDto)
		{
			Category category = _mapper.Map<Category>(categoryDto);
			_db.Add(category);
			_db.SaveChanges();
		}
		public void Update(int id, CategoryDto categoryDto)
		{
			Category? category = GetCategory(id);
			if (category != null)
			{
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
				_db.SaveChanges();
			}
		}
    }
}
