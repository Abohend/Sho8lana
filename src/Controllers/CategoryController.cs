using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Models.Dto;
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

		public CategoryController(CategoryRepository categoryRepo)
        {
			this._categoryRepo = categoryRepo;
		}
        // GET: api/<CategoryController>
        [HttpGet]
		[AllowAnonymous]
		public IActionResult Get()
		{
			try
			{
				var categories = _categoryRepo.GetAll();
				return Ok(new Response(200, categories));
			}
			catch (Exception ex)
			{
				return BadRequest(new Response(StatusCodes.Status400BadRequest, [ex.Message]));
			}
		}

		// GET api/<CategoryController>/5
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			try
			{
				var category = _categoryRepo.Get(id);
				if (category == null)
				{
					return NotFound(new Response(404, ["Category not found"]));
				}
				else
				{
					return Ok(new Response(200, category));
				}
				
			}
			catch(Exception ex)
			{
				return BadRequest(new Response(StatusCodes.Status400BadRequest, [ex.Message]));
			}
		}

		// POST api/<CategoryController>
		[HttpPost]
		public IActionResult Post([FromBody] CategoryDto categoryDto)
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
		public IActionResult Put(int id, [FromBody] CategoryDto categoryDto)
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
