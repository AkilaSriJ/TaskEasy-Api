using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Models
{
    public class WorkTaskLabel
    {
        public int WorkTaskId {  get; set; }
        public WorkTask WorkTask { get; set; }

        public int LabelId { get; set; }
        public Label Label { get; set; }

    }
}
