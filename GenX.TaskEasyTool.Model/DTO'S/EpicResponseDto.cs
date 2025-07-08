using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Model.DTO_S
{
    public class EpicResponseDto
    {
        public int Id { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string EpicName { get; set; }
        public string SummaryOfTheEpic { get; set; }
        public string EpicWorkflow { get; set; }
        public List<AssigneeDto> Assignees { get; set; }
        public string Label { get; set; }
      
    }
}
