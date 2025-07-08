using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Model.DTO_S
{
    public class ProjectCreateDto
    {
        public string ProjectName { get; set; }

        public string ProjectDescription { get; set; }

        public string CreatedByUser { get; set; }
        public List<int> AssignedUserIds { get; set; }

       
    }
}
