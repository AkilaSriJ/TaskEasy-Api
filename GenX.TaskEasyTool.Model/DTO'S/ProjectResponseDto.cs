using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Model.DTO_S
{
    public class ProjectResponseDto
    {
        public string Id { get; set; } // PROJ-001

        public string ProjectName { get; set; }

        public string ProjectDescription { get; set; }

        public string CreatedByUser { get; set; } // UserName

      
        public List<AssigneeDto> Assignees { get; set; }
    }
}
