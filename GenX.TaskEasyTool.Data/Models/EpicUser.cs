using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Models
{
    public class EpicUser
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int EpicId { get; set; }
        public Epic Epic { get; set; }
    }
}
