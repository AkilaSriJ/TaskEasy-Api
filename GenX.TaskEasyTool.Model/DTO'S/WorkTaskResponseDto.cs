using GenX.TaskEasyTool.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Model.DTO_S
{
    public class WorkTaskResponseDto
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }

        public string Type { get; set; }
        public List<string> LabelNames { get; set; }
        public string Status { get; set; }

        public int StoryPoints { get; set; }
        public int EstimatedHours { get; set; }
        public int CompletedHours { get; set; }
        public int RemainingHours { get; set; }

        public string ProjectId { get; set; }
        public int? EpicId { get; set; }
        public int? SprintId { get; set; }

        public List<AssigneeDto> Assignees { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        public string Reporter { get; set; }
    }
}
