using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }  // Admin or User
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string? OtpCode { get; set; }
        public DateTime? OtpExpiryTime { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; }
        public ICollection<EpicUser> EpicUsers { get; set; }
        public ICollection<SprintUser> SprintUsers { get; set; }
        public ICollection<WorkTaskUser> WorkTaskUsers { get; set; }

    }
}
