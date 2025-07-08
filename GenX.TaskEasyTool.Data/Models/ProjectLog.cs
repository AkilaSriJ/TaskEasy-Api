using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Models
{
    public class ProjectLog
    {
        public int Id { get; set; }
        public string ProjectId { get; set; }

        public string Action { get; set; }        
        public string PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

        public string? Description { get; set; }
    }
}
