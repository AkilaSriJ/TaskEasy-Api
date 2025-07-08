using GenX.TaskEasyTool.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Model.DTO_S
{
    public class SprintCreateDto
    {
        public string SprintName { get; set; }
        public string SprintGoal { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Status Status { get; set; }

        public string ProjectId { get; set; }
        public int CreatedByUserId { get; set; }

        public List<int> AssigneeUserIds { get; set; } 

    }
}
