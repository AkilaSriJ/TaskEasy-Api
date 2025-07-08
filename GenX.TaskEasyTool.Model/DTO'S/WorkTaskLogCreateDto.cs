using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Model.DTO_S
{
    public class WorkTaskLogCreateDto
    {
        public int WorkTaskId { get; set; }
        public int UserId { get; set; }
        public string StatusUpdate { get; set; }
        public int HoursWorked { get; set; }
        public string Comment { get; set; }
    }
}
