using AutoMapper;
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
	public class SkillController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly SkillRepository _skillRepo;

		public SkillController(IMapper mapper, SkillRepository skillRepo)
        {
			this._mapper = mapper;
			this._skillRepo = skillRepo;
		}
        // GET: api/<SkillController>
        [HttpGet]
		public IActionResult Get()
		{
			var skills = _mapper.Map<List<SkillDto>?>(_skillRepo.ReadAll());
			return Ok(new Response(200, skills));
		}

		// GET api/<SkillController>/5
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var skill = _mapper.Map<SkillDto>(_skillRepo.ReadById(id));
			if (skill != null)
				return Ok(new Response(200, skill));
			return BadRequest(new Response(404, "Skill not found"));
		}

		// POST api/<SkillController>
		[HttpPost]
		public IActionResult Post([FromBody] SkillDto skillDto)
		{
			var notDuplicated = _skillRepo.Create(_mapper.Map<Skill>(skillDto));
			if (notDuplicated)
			{
				return Ok(new Response(201));
			}
			else
			{
				return BadRequest(new Response(400, ["Skill already exists"]));
			}
		}

		// PUT api/<SkillController>/5
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] SkillDto skillDto)
		{
			var newSkill = _mapper.Map<Skill>(skillDto);
			newSkill.Id = id;
			var existed = _skillRepo.Update(newSkill);
			if (existed)
			{
				return Ok(new Response(200));
			}
			else
			{
				return BadRequest(new Response(404, ["Skill not found"]));
			}
		}

		// DELETE api/<SkillController>/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var existed = _skillRepo.Delete(id);
			if (existed)
			{
				return Ok(new Response(200));
			}
			else
			{
				return BadRequest(new Response(404, ["Skill not found"]));
			}
		}   
	}
}
