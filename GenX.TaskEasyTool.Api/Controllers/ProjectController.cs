using GenX.TaskEasyTool.Data.Interface;
using GenX.TaskEasyTool.Model.DTO_S;
using GenX.TaskEasyTool.Service.Interface;
using GenX.TaskEasyTool.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenX.TaskEasyTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;
        private readonly IProjectLogRepository _projectLogRepository;

        public ProjectController(IProjectService service, IProjectLogRepository projectLogRepository)
        {
            _service = service;
            _projectLogRepository = projectLogRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public IActionResult Create(ProjectCreateDto dto)
        {
            try
            {
                var result = _service.CreateProject(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var projects = _service.GetAllProjects();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var project = _service.GetProjectById(id);
            if (project == null) return NotFound("Project not found.");
            return Ok(project);
        }
       
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var existing = _service.GetProjectById(id);
            if (existing == null)
                return NotFound("Project not found.");

            _service.DeleteProject(id);
            return Ok("Project deleted successfully.");
        }
        [HttpGet("names")]
        public IActionResult GetAllProjectNamesAndIds()
        {
            var projects = _service.GetAllProjectNamesAndIds();
            return Ok(projects);
        }
        [HttpGet("{projectId}/logs")]
        public IActionResult GetProjectLogs(string projectId)
        {
            var logs = _projectLogRepository.GetLogsByProjectId(projectId);
            return Ok(logs);
        }
    }
}
