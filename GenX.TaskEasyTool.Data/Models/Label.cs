using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Models
{
    public class Label
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public ICollection<WorkTaskLabel> workTaskLabels { get; set; }
    }
}
