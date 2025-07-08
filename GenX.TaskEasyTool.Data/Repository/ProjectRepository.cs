using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Data.Interface;
using Microsoft.EntityFrameworkCore;
using GenX.TaskEasyTool.Data.Context;
using GenX.TaskEasyTool.Data.Models;

namespace GenX.TaskEasyTool.Data.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;
        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }
        public Project CreateProject(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return project;
        }

        public Project GetLastProject()
        {
            return _context.Projects.OrderByDescending(p => p.Id).FirstOrDefault();
        }
        public Project GetProjectById(string id)
        {
            return _context.Projects
                .Include(p => p.ProjectUsers)
                .FirstOrDefault(p => p.Id == id);
        }

        public User GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public User GetUserByUserName(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
        
        public void AssignUsersToProject(string projectId, List<int> userIds)
        {
            foreach (var userId in userIds)
            {
                if (!_context.ProjectUsers.Any(pu => pu.ProjectId == projectId && pu.UserId == userId))
                {
                    _context.ProjectUsers.Add(new ProjectUser
                    {
                        ProjectId = projectId,
                        UserId = userId
                    });
                }
            }
            _context.SaveChanges();
        }

        public List<string> GetAssignedUsernames(string projectId)
        {
            return _context.ProjectUsers
                .Where(pu => pu.ProjectId == projectId)
                .Select(pu => pu.User.Username)
                .ToList();
        }
        public List<Project> GetAllWithUsers()
        {
            return _context.Projects
                .Include(p => p.ProjectUsers).ThenInclude(pu => pu.User)
                .Include(p => p.CreatedByUser)
                .ToList();
        }

        public Project GetByIdWithUsers(string id)
        {
            return _context.Projects
                .Include(p => p.ProjectUsers).ThenInclude(pu => pu.User)
                .Include(p => p.CreatedByUser)
                .FirstOrDefault(p => p.Id == id);
        }
        public List<Project> GetAllProjects()
        {
            return _context.Projects.ToList();
        }


        public void Delete(Project project)
        {
            try
            {
                _context.Projects.Remove(project);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        public List<Project> GetAllProjectNamesAndIds()
        {
            return _context.Projects
                .Select(p => new Project { Id = p.Id, ProjectName = p.ProjectName ,ProjectDescription=p.ProjectDescription})
                .ToList();
        }


    }
}
