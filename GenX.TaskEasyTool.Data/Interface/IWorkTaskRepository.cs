using GenX.TaskEasyTool.Data.Models;
using GenX.TaskEasyTool.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace GenX.TaskEasyTool.Data.Interface
{
    public interface IWorkTaskRepository
    {
        WorkTask Create(WorkTask task, List<int> assigneeIds);
        WorkTask GetById(int id);
        void Update(WorkTask task);
        List<User> GetAssigneeUsers(int taskId);
        List<WorkTask> GetAll();
        WorkTask GetTaskWithUsers(int taskId);
        User GetUserById(int userId);

        List<WorkTask> GetProductBacklog(string projectId);
        List<WorkTask> GetAssignedTasksInSprint(int sprintId);
        Label GetLabelByName(string name);
        void AddLabel(Label label);
        void ClearAssignees(int taskId);
        void AssignUserToTask(int taskId, int userId);
        void Delete(int taskId);

        void SaveChanges();

    }
}
