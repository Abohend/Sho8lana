using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sho8lana.API.Exceptions;
using Sho8lana.API.Services;
using Sho8lana.DataAccess.Repositories;
using Sho8lana.Entities.Models;
using Sho8lana.Entities.Models.Dto.Category;

namespace Sho8lana.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        private readonly FileService _imageService;

        public CategoryController(CategoryRepository categoryRepo, IMapper mapper, FileService imageService)
        {
            this._categoryRepo = categoryRepo;
            this._mapper = mapper;
            this._imageService = imageService;
        }

        #region Helpers
        private string? GetImageUrl(string path)
        {
            if (path != null)
                return $"{Request.Scheme}://{Request.Host}/{path}";
            return null;
        }
        #endregion

        // GET: api/<CategoryController>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var categories = _categoryRepo.GetAll();
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

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            var category = _categoryRepo.Get(id)
                ?? throw new NotFoundException("Category Id not found");

            var categoryDto = _mapper.Map<GetCategoryDto>(category);
            categoryDto.ImageUrl = GetImageUrl(category.ImagePath);

            return Ok(new Response(200, categoryDto));
        }

        // POST api/<CategoryController>
        [HttpPost]
        public IActionResult Post([FromForm] CreateCategoryDto categoryDto)
        {
            var imagePath = _imageService.UploadImage("category", categoryDto.Image);
            _categoryRepo.Create(categoryDto, imagePath);
            return Ok(new Response(201));
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] UpdateCategoryDto categoryDto)
        {
            string? imagePath = null;
            if (categoryDto.Image != null)
            {
                _imageService.DeleteImage(_categoryRepo.Get(id)!.ImagePath);
                imagePath = _imageService.UploadImage("category", categoryDto.Image);
            }
            _categoryRepo.Update(id, categoryDto, imagePath);
            return Ok(new Response(201));
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _imageService.DeleteImage(_categoryRepo.Get(id)!.ImagePath);
            _categoryRepo.Delete(id);
            return Ok(new Response(200));
        }
    }
}
