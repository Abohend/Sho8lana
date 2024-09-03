using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sho8lana.Entities.Models;
using Sho8lana.DataAccess.Repositories;
using System.Security.Claims;

namespace Sho8lana.API.Controllers
{
    [Authorize(Roles = "Client, Freelancer")]
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveredProductsController : ControllerBase
    {
        private readonly DeliveredProductRepository _productRepo;
        private readonly ProjectRepository _projectRepo;
        private readonly JobRepository _jobRepo;

        public DeliveredProductsController(DeliveredProductRepository productRepo,
            ProjectRepository projectRepo, JobRepository jobRepo)
        {
            this._productRepo = productRepo;
            this._projectRepo = projectRepo;
            this._jobRepo = jobRepo;
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

        [HttpGet("readprojectproduct/{projectId}")]
        public IActionResult GetProjectProduct(int projectId)
        {
            // ensure project existance
            var project = _projectRepo.Read(projectId);
            if (project == null)
                return BadRequest(new Response(400, ["Invalid Project Id"]));

            // either project owner or freelancer who took the project can view the delivered product
            else if ((GetRole() == "Client" && GetId() != project.ClientId) &&
                (GetRole() == "Freelancer" && GetId() != _projectRepo.ReadProjectTakerId(projectId)))
            {
                return Unauthorized(new Response(401, ["Not Allowed to Update this Project"]));
            }

            return Ok(new Response(200, _productRepo.ReadProjectProduct(projectId)));
        }


        [HttpGet("readjobproduct/{jobId}")]
        public IActionResult GetJobProduct(int jobId)
        {
            // ensure job existance
            var job = _jobRepo.Read(jobId);
            if (job == null)
                return BadRequest(new Response(400, ["Invalid Job Id"]));

            // either job owner or freelancer who took the job can view the delivered product
            else if (GetRole() == "Freelancer" && (GetId() != _jobRepo.ReadJobTakerId(jobId) || GetId() != _jobRepo.ReadJobOwnerId(jobId)) || GetRole() == "Client")
            {
                return Unauthorized(new Response(401, ["Not Allowed to Update this Job"]));
            }

            return Ok(new Response(200, _productRepo.ReadJobProduct(jobId)));
        }


        [Authorize(Roles = "Freelancer")]
        [HttpPost("project/{projectId}")]
        public IActionResult PostProjectProduct(int projectId, [FromBody] string GitHubUrl)
        {
            // ensure project existance
            var freelancerId = GetId();
            var project = _projectRepo.Read(projectId);
            if (project == null)
                return BadRequest(new Response(400, ["Invalid Project Id"]));

            // check project taker
            else if (_projectRepo.ReadProjectTakerId(projectId) != freelancerId)
            {
                return Unauthorized(new Response(401, ["Not Allowed to Update this Project"]));
            }

            // check if product already exists for this project
            if (_productRepo.ReadProjectProduct(projectId) != null)
            {
                return BadRequest(new Response(400, ["Product already delivered for this project"]));
            }
            
            // make sure the variable GitHubUrl is url of a valid github repository
            if (!Uri.IsWellFormedUriString(GitHubUrl, UriKind.RelativeOrAbsolute) && GitHubUrl.ToLower().Contains("github.com"))
            {
                return BadRequest(new Response(400, ["Invalid GitHub Url"]));
            }

            _productRepo.CreateProjectProduct(projectId, GitHubUrl);
            return Ok(new Response(200));
        }
        

        [Authorize(Roles = "Freelancer")]
        [HttpPost("job/{jobId}")]
        public IActionResult PostJobProduct(int jobId, [FromBody] string GitHubUrl)
        {
            // ensure job existance
            var freelancerId = GetId();
            var job = _jobRepo.Read(jobId);
            if (job == null)
                return BadRequest(new Response(400, ["Invalid Job Id"]));

            // check job taker
            else if (_jobRepo.ReadJobTakerId(jobId) != freelancerId)
            {
                return Unauthorized(new Response(401, ["Not Allowed to Update this Job"]));
            }

            // check if job already exists for this project
            if (_productRepo.ReadJobProduct(jobId) != null)
            {
                return BadRequest(new Response(400, ["Product already delivered for this job"]));
            }
            
            // make sure the variable GitHubUrl is url of a valid github repository
            if (!Uri.IsWellFormedUriString(GitHubUrl, UriKind.RelativeOrAbsolute) && GitHubUrl.ToLower().Contains("github.com"))
            {
                return BadRequest(new Response(400, ["Invalid GitHub Url"]));
            }

            _productRepo.CreateJobProduct(jobId, GitHubUrl);
            return Ok(new Response(200));
        }


        [Authorize(Roles = "Freelancer")]
        [HttpPost("verifyJob/{jobId}")]
        public IActionResult VerifyJob(int jobId)
        {
            var freelancerId = GetId();

            // ensure job existance
            var job = _jobRepo.Read(jobId);
            if (job == null)
                return BadRequest(new Response(400, ["Invalid Job Id"]));


            // verify the owner of the job
            else if (_jobRepo.ReadJobOwnerId(jobId) != freelancerId)
            {
                return Unauthorized(new Response(401, ["Not Allowed to Update this Job"]));
            }

            // ensure product existance
            var product = _productRepo.ReadJobProduct(jobId);
            if (product == null)
            {
                return NotFound(new Response(404, ["Delivered product for this job not found"]));
            }

            _productRepo.VerifyJobProduct(jobId);

            // Todo: release payment
            return Ok(new Response(200));
        }


        [Authorize(Roles = "Client")]
        [HttpPost("verifyProject/{projectId}")]
        public IActionResult VerifyProject(int projectId)
        {
            // check project existance
            var project = _projectRepo.Read(projectId);
            if (project == null)
                return BadRequest(new Response(400, ["Invalid Project Id"]));

            // verify the owner of the project
            if (GetId() != project.ClientId)
            {
                return Unauthorized(new Response(401, ["Not Allowed to Update this Project"]));
            }

            // ensure product existance
            var product = _productRepo.ReadProjectProduct(projectId);
            if (product == null)
            {
                return NotFound(new Response(404, ["Delivered product for this job not found"]));
            }

            _productRepo.VerifyProjectProduct(projectId);

            // Todo: release payment
            return Ok(new Response(200));
        }
    }
}
