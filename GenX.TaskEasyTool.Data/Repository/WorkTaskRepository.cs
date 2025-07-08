using GenX.TaskEasyTool.Data.Context;
using GenX.TaskEasyTool.Data.Interface;
using GenX.TaskEasyTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace GenX.TaskEasyTool.Data.Repository
{
    public class WorkTaskRepository: IWorkTaskRepository
    {
        private readonly AppDbContext _context;

        public WorkTaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public WorkTask Create(WorkTask task, List<int> assigneeIds)
        {
            try
            {
                _context.Tasks.Add(task);
                _context.SaveChanges();

                foreach (var userId in assigneeIds)
                {
                    _context.WorkTaskUsers.Add(new WorkTaskUser
                    {
                        WorkTaskId = task.Id,
                        UserId = userId
                    });
                }

                _context.SaveChanges();
                Console.WriteLine("Task saved successfully with ID: " + task.Id);
                return task;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                return null;
            }
            
        }
        public List<WorkTask> GetAll()
        {
            return _context.Tasks

                .Include(t => t.WorkTaskUsers)
                    .ThenInclude(wtu => wtu.User) 
                .ToList();
        }

        public WorkTask GetById(int id)
        {
            return _context.Tasks
                 .Include(t => t.WorkTaskUsers)
                     .ThenInclude(wtu => wtu.User)
                 .Include(t => t.Project)
                 .Include(t => t.WorkTaskLabels)
                    .ThenInclude(l=>l.Label)
                 .Include(t => t.Epic)
                 .Include(t => t.Sprint)
                 .FirstOrDefault(t => t.Id == id);
        }

        public void Update(WorkTask task)
        {
            try
            {
                _context.Tasks.Update(task);
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void AssignUsersToWorkTask(int taskId, List<int> assigneeIds)
        {
            foreach (var userId in assigneeIds)
            {
                bool alreadyExists = _context.WorkTaskUsers.Any(wt => wt.WorkTaskId == taskId && wt.UserId == userId);
                if (!alreadyExists)
                {
                    _context.WorkTaskUsers.Add(new WorkTaskUser
                    {
                        WorkTaskId = taskId,
                        UserId = userId
                    });
                }
            }

            _context.SaveChanges();
        }
        public List<User> GetAssigneeUsers(int taskId)
        {
            return _context.WorkTaskUsers
                .Where(wt => wt.WorkTaskId == taskId)
                .Select(wt => wt.User)
                .ToList();
        }
        public WorkTask GetTaskWithUsers(int taskId)
        {
            return _context.Tasks
                .Include(t => t.WorkTaskUsers)
                    .ThenInclude(wtu => wtu.User)
                .FirstOrDefault(t => t.Id == taskId);
            
        }
        public User GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public List<WorkTask> GetProductBacklog(string projectId)
        {
            return _context.Tasks
                .Include(t => t.WorkTaskLabels)
                .ThenInclude(l=>l.Label)
                .Include(t => t.Project)
                .Include(t => t.WorkTaskUsers)
                .Where(t => t.ProjectId == projectId && t.SprintId == null)
                .ToList();
        }

        public List<WorkTask> GetAssignedTasksInSprint(int sprintId)
        {
            return _context.Tasks
                .Include(t=>t.WorkTaskLabels)
                .ThenInclude(l=>l.Label)
                .Include(t => t.Sprint)
                .Include(t => t.WorkTaskUsers)
                .Where(t => t.SprintId == sprintId && t.WorkTaskUsers.Any())
                .ToList();
        }
        public Label GetLabelByName(string name)
        {
            return _context.Labels.FirstOrDefault(l => l.Name == name);
        }
        public void AddLabel(Label label)
        {
            _context.Labels.Add(label);
            _context.SaveChanges();
        }

        public void ClearAssignees(int taskId)
        {
            var assignees = _context.WorkTaskUsers.Where(a => a.WorkTaskId == taskId);
            _context.WorkTaskUsers.RemoveRange(assignees);
            _context.SaveChanges();
        }

        public void AssignUserToTask(int taskId, int userId)
        {
            var assignment = new WorkTaskUser
            {
                WorkTaskId = taskId,
                UserId = userId
            };
            _context.WorkTaskUsers.Add(assignment);
            _context.SaveChanges();
        }
        public void Delete(int taskId)
        {
            var task = _context.Tasks
                .Include(t => t.WorkTaskUsers) 
                .FirstOrDefault(t => t.Id == taskId);

            if (task != null)
            {
                _context.WorkTaskUsers.RemoveRange(task.WorkTaskUsers);
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }


    }
}
