using GenX.TaskEasyTool.Model.DTO_S;
using GenX.TaskEasyTool.Service.Interface;
using GenX.TaskEasyTool.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GenX.TaskEasyTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkTaskController : ControllerBase
    {
        private readonly IWorkTaskService _service;

        public WorkTaskController(IWorkTaskService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create([FromBody] WorkTaskCreateDto dto)
        {

            var response = _service.CreateWorkTask(dto); 

            if (response == null)
                return BadRequest("Task creation failed.");

            return Ok(response); 
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var tasks = _service.GetAllWorkTasks();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var task = _service.GetWorkTaskById(id);
            if (task == null) return NotFound("Task not found.");
            return Ok(task);
        }

        [HttpPost("log")]
        public IActionResult AddLog([FromBody] WorkTaskLogCreateDto dto)
        {
            _service.AddWorkTaskLog(dto);
            return Ok(new
            {
                workTaskId = dto.WorkTaskId,
                statusUpdate = dto.StatusUpdate,
                hoursWorked = dto.HoursWorked,
                comment = dto.Comment
            });
        }

        [HttpPut("log")]
        public IActionResult UpdateLog([FromBody] WorkTaskLogUpdateDto dto)
        {
            _service.UpdateWorkTaskLog(dto);
            return Ok(new
            {
                workTaskId = dto.Id,
                statusUpdate = dto.StatusUpdate,
                hoursWorked = dto.HoursWorked,
                comment = dto.Comment
            });
        }

        [HttpGet("log")]
        public IActionResult GetAllLogs()
        {
            var logs = _service.GetAllLogs();
            return Ok(logs);
        }

        [HttpGet("log/{id}")]
        public IActionResult GetLogById(int id)
        {
            var log = _service.GetLogById(id);
            if (log == null) return NotFound("Log not found.");
            return Ok(log);
        }
        
        [HttpPut("replace-assignees")]
        public IActionResult ReplaceAssignees([FromBody] WorkTaskAssigneeUpdateDto dto)
        {
            var response = _service.ReplaceAssignees(dto);
            if (response == null)
                return NotFound("Task not found");

            return Ok(response); 
        }
        [HttpGet("backlog/project/{projectId}")]
        public IActionResult GetProductBacklog(string projectId)
        {
            var backlogTasks = _service.GetProductBacklog(projectId); 
            return Ok(backlogTasks);
        }

        [HttpGet("backlog/assigned/sprint/{sprintId}")]
        public IActionResult GetassignedTasksInSprint(int sprintId)
        {
            var unassignedTasks = _service.GetAssignedTasksInSprint(sprintId); 
            return Ok(unassignedTasks);
        }

        [HttpGet("{taskId}/logs")]
        public IActionResult GetTaskLogs(int taskId)
        {
            var logs = _service
                .GetAllLogs()
                .Where(l => l.WorkTaskId == taskId)
                .ToList();

            return Ok(logs);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.DeleteWorkTask(id);
            return Ok(new { message = $"Task with ID {id} deleted successfully." });
        }

    }
}
