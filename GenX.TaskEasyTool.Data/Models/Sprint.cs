using GenX.TaskEasyTool.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Models
{
    public class Sprint
    {
        public int Id { get; set; }
        public string SprintName { get; set; }
        public string SprintGoal { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Status Status { get; set; } // enum or string like "Planned", "Active", etc.

        public string ProjectId { get; set; }
        public Project Project { get; set; }

        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SprintUser> SprintUsers { get; set; }
        public ICollection<WorkTask> Tasks { get; set; }
    }
}
