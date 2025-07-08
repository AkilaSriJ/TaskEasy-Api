using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Models
{
    public class WorkTaskLog
    {
        public int Id { get; set; }

        public int WorkTaskId { get; set; }
        public WorkTask WorkTask { get; set; }
        public int UserId { get; set; } 
        public User User { get; set; }
        public string StatusUpdate { get; set; }
        public int HoursWorked { get; set; }
        public string Comment { get; set; }

        public DateTime LoggedAt { get; set; } = DateTime.Now;
    }
}
