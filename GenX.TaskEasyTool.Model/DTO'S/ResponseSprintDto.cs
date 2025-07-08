using GenX.TaskEasyTool.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Model.DTO_S
{
    public class ResponseSprintDto
    {
        public int Id { get; set; }
        public string SprintName { get; set; }
        public string SprintGoal { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; }

        public string ProjectId { get; set; }
        public string ProjectName { get; set; }

        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }

        public List<AssigneeDto> Assignees { get; set; }
        public List<UserActiveTaskDto> UserActiveTasks { get; set; }

        public int TotalSprintDays => (EndDate.Date - StartDate.Date).Days + 1;

        public int DaysPassed =>
            DateTime.UtcNow.Date < StartDate.Date ? 0 :
            (DateTime.UtcNow.Date > EndDate.Date ? TotalSprintDays :
            (DateTime.UtcNow.Date - StartDate.Date).Days);

        public int DaysRemaining =>
            DateTime.UtcNow.Date >= EndDate.Date ? 0 :
            (EndDate.Date - DateTime.UtcNow.Date).Days;

        public double SprintProgressPercentage =>
            TotalSprintDays == 0 ? 0 :
            Math.Min(100, Math.Round((double)DaysPassed / TotalSprintDays * 100, 2));
        public int TeamMembersCount { get; set; }
        public int ActiveTaskCount { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTaskCount { get; set; }
        public int HighPriorityTasks { get; set; }
        public int TotalHoursLogged { get; set; }
    }
}
