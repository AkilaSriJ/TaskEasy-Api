using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Models
{
    public class SprintUser
    {
        public int SprintId { get; set; }
        public Sprint Sprint { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
