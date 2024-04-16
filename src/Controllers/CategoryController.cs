using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Models.Dto.Category;
using src.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace src.Controllers
{
    [Authorize(Roles = "Admin")]
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly CategoryRepository _categoryRepo;
		private readonly IMapper _mapper;

		public CategoryController(CategoryRepository categoryRepo, IMapper mapper)
		{
			this._categoryRepo = categoryRepo;
			this._mapper = mapper;
		}

		#region Helpers
		private string GetImageUrl(string path)
		{
			return $"{Request.Scheme}://{Request.Host}/{path}";
		}
		#endregion

		// GET: api/<CategoryController>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult Get()
		{
			try
			{
				var categories = _categoryRepo.ReadAll();
				var result = categories?
					.Select(c => new GetCategoryDto
					{
						Id = c.Id,
						Name = c.Name,
						ImageUrl = GetImageUrl(c.ImagePath)
					})
					.ToList();
				return Ok(new Response(200, result));
			}
			catch (Exception ex)
			{
				return BadRequest(new Response(StatusCodes.Status400BadRequest, [ex.Message]));
			}
		}

		// GET api/<CategoryController>/5
		[HttpGet("{id}")]
		[AllowAnonymous]
		public IActionResult Get(int id)
		{
			try
			{
				var category = _categoryRepo.ReadById(id);
				if (category == null)
				{
					return NotFound(new Response(404, ["Category not found"]));
				}
				else
				{
					var categoryDto = _mapper.Map<GetCategoryDto>(category);
					categoryDto.ImageUrl = GetImageUrl(category.ImagePath);
					return Ok(new Response(200, categoryDto));
				}

			}
			catch (Exception ex)
			{
				return BadRequest(new Response(StatusCodes.Status400BadRequest, [ex.Message]));
			}
		}

		// POST api/<CategoryController>
		[HttpPost]
		public IActionResult Post([FromForm] CreateCategoryDto categoryDto)
		{
			try
			{
				_categoryRepo.Create(categoryDto);
				return Ok(new Response(201));
			}
			catch (Exception ex)
			{
				return BadRequest(new Response(StatusCodes.Status400BadRequest, [ex.Message, ex.InnerException!.Message]));
			}
		}

		// PUT api/<CategoryController>/5
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromForm] UpdateCategoryDto categoryDto)
		{
			try
			{
				_categoryRepo.Update(id, categoryDto);
				return Ok(new Response(201));
			}
			catch (Exception ex)
			{
				return BadRequest(new Response(StatusCodes.Status400BadRequest, [ex.Message]));
			}
		}

		// DELETE api/<CategoryController>/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			try
			{
				_categoryRepo.Delete(id);
				return Ok(new Response(200));
			}
			catch (Exception ex)
			{
				return BadRequest(new Response(StatusCodes.Status400BadRequest, [ex.Message]));
			}
		}
	}
}
