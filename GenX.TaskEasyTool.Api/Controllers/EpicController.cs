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
    public class EpicController : ControllerBase
    {
        private readonly IEpicService _service;
        private readonly IEpicLogRepository _epicLogRepository;

        public EpicController(IEpicService service, IEpicLogRepository epicLogRepository)
        {
            _service = service;
            _epicLogRepository = epicLogRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public IActionResult CreateEpic([FromBody] EpicCreateDto dto)
        {
            try
            {
                var result = _service.CreateEpic(dto);
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
            var epics = _service.GetAllEpics();
            return Ok(epics);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var epic = _service.GetEpicById(id);
            if (epic == null)
                return NotFound("Epic not found.");
            return Ok(epic);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var epic = _service.GetEpicById(id);
            if (epic == null)
                return NotFound("Epic not found.");

            _service.DeleteEpic(id);
            return Ok("Epic deleted successfully.");
        }
        [HttpGet("{epicId}/logs")]
        public IActionResult GetEpicLogs(int epicId)
        {
            var logs = _epicLogRepository.GetLogsByEpicId(epicId);
            return Ok(logs);
        }
    }
}
