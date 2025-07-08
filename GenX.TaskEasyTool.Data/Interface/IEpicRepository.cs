using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Data.Models;

namespace GenX.TaskEasyTool.Data.Interface
{
    public interface IEpicRepository
    {
        Epic CreateEpic(Epic epic);
        void AssignUsersToEpic(int epicId, List<int> assigneeIds);
        Project GetProjectById(string projectId);
        User GetAssigneeById(int assigneeId);
        List<Epic> GetAllWithUsersAndProject();
        Epic GetByIdWithUsersAndProject(int id);
        Epic GetById(int id);
        void Delete(Epic epic);


    }
}
