using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Common.Enum;

namespace GenX.TaskEasyTool.Data.Models
{
    public class Epic
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EpicName { get; set; }

        [Required]
        public string SummaryOfTheEpic { get; set; }

        [Required]
        public EpicWorkflow EpicWorkflow { get; set; } = EpicWorkflow.ToDo;

        [Required]
        public string? ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }

        public int LabelId { get; set; }
        public Label Label { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<EpicUser> EpicUsers { get; set; }
        
    }
}
