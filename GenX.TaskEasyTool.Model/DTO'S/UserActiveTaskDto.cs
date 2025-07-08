using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Model.DTO_S
{
    public class UserActiveTaskDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ActiveTaskCount { get; set; }
    }
}
