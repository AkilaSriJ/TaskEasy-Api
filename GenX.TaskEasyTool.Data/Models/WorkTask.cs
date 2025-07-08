using GenX.TaskEasyTool.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Models
{
    public class WorkTask
    {
        public int Id { get; set; }

        public string Summary{ get; set; }
        public string Description { get; set; }

        public Types Type { get; set; }
       

        public Status Status { get; set; }

        public int StoryPoints { get; set; }
        public int EstimatedHours { get; set; }
        public int CompletedHours { get; set; }
        public int RemainingHours { get; set; }

        public string ProjectId { get; set; }
        public Project Project { get; set; }

        public int? EpicId { get; set; }
        public Epic Epic { get; set; }

        public int? SprintId { get; set; }
        public Sprint Sprint { get; set; }

        public ICollection<WorkTaskUser> WorkTaskUsers { get; set; }
        public ICollection<WorkTaskLog> WorkTaskLogs { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        public string Reporter { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<WorkTaskLabel> WorkTaskLabels { get; set; }

    }
}
