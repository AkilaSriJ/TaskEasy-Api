using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Model.DTO_S
{
    public class WorkTaskAssigneeUpdateDto
    {
        public int TaskId { get; set; }
        public List<int> AssigneeIds { get; set; }  
    }

    
}
