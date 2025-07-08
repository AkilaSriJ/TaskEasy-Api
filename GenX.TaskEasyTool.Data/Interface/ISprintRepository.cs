using GenX.TaskEasyTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GenX.TaskEasyTool.Data.Interface
{
    public interface ISprintRepository
    {
        Sprint CreateSprint(Sprint sprint, List<int> assigneeUserIds);
        List<Sprint> GetAllSprints();
        Sprint GetSprintById(int id);
        void UpdateSprint(int id, Sprint updatedSprint, List<int> assigneeUserIds);
        bool DeleteSprint(int id);
        User GetAssigneeById(int userId);
        Project GetProjectById(string projectId);
        void AssignUsersToSprint(int sprintId, List<int> assigneeIds);
        Sprint GetSprintWithMetricsById(int id);

        List<WorkTask> GetTasksBySprintId(int sprintId);
        List<WorkTaskLog> GetLogsBySprintId(int sprintId);

    }
}
