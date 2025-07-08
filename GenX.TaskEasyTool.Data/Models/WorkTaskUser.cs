using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Models
{
    public class WorkTaskUser
    {
        public int WorkTaskId { get; set; }
        public WorkTask WorkTask { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
