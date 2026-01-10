using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sho8lana.API.Exceptions;
using Sho8lana.DataAccess.Repositories;
using Sho8lana.Entities.Models;
using Sho8lana.Entities.Models.Dto.Project;
using System.Security.Claims;


namespace Sho8lana.API.Controllers
{
    //[Authorize(Roles = "Client")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectRepository _projectRepo;
        private readonly CategoryRepository _categoryRepo;
        private readonly SkillRepository _skillRepo;

        public ProjectController(ProjectRepository projectRepo,
            CategoryRepository categoryRepo,
            SkillRepository skillRepository)
        {
            _projectRepo = projectRepo;
            _categoryRepo = categoryRepo;
            this._skillRepo = skillRepository;
        }

        #region Helpers
        private string GetRole()
        {
            return User.FindFirst(ClaimTypes.Role)!.Value.ToLower();
        }
        private string GetId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
        #endregion


        // GET: api/<ProjectController>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            var projects = _projectRepo.ReadAll();
            return Ok(new Response(200, result: projects));
        }

        // GET api/<ProjectController>/5
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var project = _projectRepo.ReadWithSkills(id)
                ?? throw new NotFoundException("Project Id not found");
            return Ok(new Response(200, project));
        }

        [AllowAnonymous]
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var projects = _projectRepo.ReadWithSkills(name);
            return Ok(new Response(200, projects));
        }

        [Authorize(Roles = "Freelancer, Admin")]
        [HttpGet("freelancer/{freelancerId}")]
        // return all project for a freelancer
        public IActionResult Get(string freelancerId)
        {
            if (GetId() != freelancerId)
            {
                throw new UnauthorizedAccessException("You are not allowed to view projects of other freelancers");
            }
            var projects = _projectRepo.ReadAll(freelancerId);
            return Ok(new Response(200, projects));
        }

        //POST api/<ProjectController>
        [Authorize(Roles = "Client, Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] CreateProjectDto projectDto)
        {
            // check Category
            var category = _categoryRepo.Get(projectDto.CategoryId);
            if (category == null)
            {
                throw new NotFoundException("Category id is not valid");
            }

            List<Skill>? skills = null;

            if (projectDto.RequiredSkillsId != null)
            {
                skills = new List<Skill>();
                foreach (var skillId in projectDto.RequiredSkillsId)
                {
                    var skill = _skillRepo.ReadById(skillId);
                    if (skill == null)
                    {
                        throw new NotFoundException("Enter valid skills");
                    }
                    else
                    {
                        skills.Add(skill);
                    }
                }
            }

            //ToDo disable automapping for category and assign categoryId here.
            _projectRepo.Create(GetId(), skills, projectDto);
            return Ok(new Response(201));
        }


        // PUT api/<ProjectController>/5
        [Authorize(Roles = "Client")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CreateProjectDto projectDto)
        {
            // TODO authorize
            // check category existence
            var category = _categoryRepo.Get(projectDto.CategoryId);
            if (category == null)
            {
                throw new NotFoundException("Category id is not valid");
            }

            var project = _projectRepo.Read(id)
            ?? throw new NotFoundException("Project Id not found");

            // check the updator
            if (GetRole() == "Client" && GetId() != project.ClientId)
                throw new UnauthorizedAccessException();

            // validating skills id
            foreach (var skillId in projectDto.RequiredSkillsId!)
            {
                var skill = _skillRepo.ReadById(skillId);
                if (skill == null)
                {
                    throw new NotFoundException("Enter valid skills");
                }
            }

            var result = _projectRepo.Update(id, projectDto);

            return Ok(new Response(201));

        }


        // DELETE api/<ProjectController>/5
        [Authorize(Roles = "Admin, Client")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var project = _projectRepo.Read(id)
                ?? throw new NotFoundException("Project Id not found");

            // if client
            if (GetRole() == "Client" && GetId() != project.ClientId)
                throw new UnauthorizedAccessException();

            else
            {
                _ = _projectRepo.Delete(id);
                return Ok(new Response(200));
            }
        }
    }
}
