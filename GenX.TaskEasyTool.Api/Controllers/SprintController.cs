using GenX.TaskEasyTool.Data.Interface;
using GenX.TaskEasyTool.Model.DTO_S;
using GenX.TaskEasyTool.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenX.TaskEasyTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SprintController : ControllerBase
    {
        private readonly ISprintService _sprintService;
        private readonly ISprintLogRepository _sprintLogRepository;
        public SprintController(ISprintService sprintService,ISprintLogRepository sprintLogRepository)
        {
            _sprintService = sprintService;
            _sprintLogRepository = sprintLogRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<ResponseSprintDto> CreateSprint([FromBody] SprintCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Sprint data is required.");

            var createdSprint = _sprintService.CreateSprint(dto);
            return CreatedAtAction(nameof(GetSprintById), new { id = createdSprint.Id }, createdSprint);
        }

  
        [HttpGet("{id}")]
        public ActionResult<ResponseSprintDto> GetSprintById(int id)
        {
            var sprint = _sprintService.GetSprintById(id);
            if (sprint == null)
                return NotFound($"Sprint with ID {id} not found.");

            return Ok(sprint);
        }

       
        [HttpGet]
        public ActionResult<List<ResponseSprintDto>> GetAllSprints()
        {
            var sprints = _sprintService.GetAllSprints();
            return Ok(sprints);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSprint(int id, [FromBody] SprintCreateDto dto)
        {
            var existing = _sprintService.GetSprintOverviewById(id);
            if (existing == null)
                return NotFound($"Sprint with ID {id} not found.");

            _sprintService.UpdateSprint(id, dto);
            return NoContent();
        }

        [HttpGet("overview/{id}")]
        public IActionResult GetSprintOverview(int id)
        {
            var overview = _sprintService.GetSprintOverviewById(id);
            if (overview == null) return NotFound();
            return Ok(overview);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSprint(int id)
        {
            var result = _sprintService.DeleteSprint(id);
            if (!result)
                return NotFound($"Sprint with ID {id} not found.");

            return NoContent();
        }
        [HttpGet("{sprintId}/logs")]
        public IActionResult GetSprintLogs(int sprintId)
        {
            var logs = _sprintLogRepository.GetLogsBySprintId(sprintId);
            return Ok(logs);
        }
    }
}
