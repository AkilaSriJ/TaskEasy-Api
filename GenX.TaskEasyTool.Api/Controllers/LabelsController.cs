using GenX.TaskEasyTool.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GenX.TaskEasyTool.Data.Models;
using GenX.TaskEasyTool.Model.DTO_S;

namespace GenX.TaskEasyTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelService _labelService;

        public LabelsController(ILabelService labelService)
        {
            _labelService = labelService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var labels = _labelService.GetAllLabels();
            return Ok(labels);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] LabelDto dto)
        {
            var label = new Label { Name = dto.Name };

            var result = _labelService.CreateLabel(label);
            return Ok(result); 
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _labelService.DeleteLabel(id);
            return Ok();
        }

    }
}
