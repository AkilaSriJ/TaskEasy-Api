using GenX.TaskEasyTool.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Data.Models;

namespace GenX.TaskEasyTool.Model.DTO_S
{
    public class WorkTaskCreateDto
    {
        public string Summary { get; set; }
        public string Description { get; set; }

        public Types Type { get; set; }
        public List<string> LabelNames { get; set; }
        public Status Status { get; set; }

        public int StoryPoints { get; set; }
        public int EstimatedHours { get; set; }

        public string ProjectId { get; set; }
        public int? EpicId { get; set; }
        public int? SprintId { get; set; }

        public int CreatedByUserId { get; set; }  

        public List<int> AssigneeUserIds { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

    }
}
