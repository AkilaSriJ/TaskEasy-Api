using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Models
{
    public class ProjectUser
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public string ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
