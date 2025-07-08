using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Common.Enum;
using GenX.TaskEasyTool.Data.Models;

namespace GenX.TaskEasyTool.Model.DTO_S
{
    public class EpicCreateDto
    {
        public string ProjectId { get; set; }
        public string EpicName { get; set; }
        public string SummaryOfTheEpic { get; set; }
        public EpicWorkflow EpicWorkflow { get; set; } = EpicWorkflow.ToDo;
        public List<int> AssigneeIds { get; set; }
        public LabelDto Label { get; set; }
        public string CreatedByUser { get; set; }

    }
}
