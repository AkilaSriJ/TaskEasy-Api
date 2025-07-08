using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Model.DTO_S
{
    public class WorkTaskLogResponseDto
    {
        public int Id { get; set; }
        public int WorkTaskId { get; set; }
        public string StatusUpdate { get; set; }
        public int HoursWorked { get; set; }
        public string Comment { get; set; }

        public string TaskTitle { get; set; }
    }
}
