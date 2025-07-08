using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Models
{
    public class Project
    {
        [Key]
        public string Id { get; set; } // PROJ-001, etc.

        [Required]
        public string ProjectName { get; set; }

        public string ProjectDescription { get; set; }

        public int CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public User CreatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Epic> Epics { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; }
    }
}
