using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Data.Models;

namespace GenX.TaskEasyTool.Data.Interface
{
    public interface IProjectRepository
    {
        Project CreateProject(Project project);
        Project GetLastProject();
     
        Project GetProjectById(string id);

        User GetUserById(int userId);
        User GetUserByUserName(string username);
      
        void AssignUsersToProject(string projectId, List<int> userIds); 
        List<string> GetAssignedUsernames(string projectId);
        List<Project> GetAllWithUsers();
        Project GetByIdWithUsers(string id);
        List<Project> GetAllProjects();
        void Delete(Project project);

        List<Project> GetAllProjectNamesAndIds();

    }
}
