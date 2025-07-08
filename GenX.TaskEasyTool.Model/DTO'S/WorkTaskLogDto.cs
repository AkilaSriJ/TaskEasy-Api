using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Model.DTO_S
{
    public class WorkTaskLogDto
    {
        public string TaskTitle { get; set; }
        public string PerformedBy { get; set; }
        public string StatusUpdate { get; set; }
        public string Comment { get; set; }
        public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
    }
}
