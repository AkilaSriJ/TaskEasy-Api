using GenX.TaskEasyTool.Data.Context;
using GenX.TaskEasyTool.Data.Interface;
using GenX.TaskEasyTool.Data.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Repository
{
    public class SprintRepository:ISprintRepository
    {
        private readonly AppDbContext _context;

        public SprintRepository(AppDbContext context)
        {
            _context = context;
        }

        public Sprint CreateSprint(Sprint sprint, List<int> assigneeUserIds)
        {
            // Add Sprint entity
            _context.Sprints.Add(sprint);
            _context.SaveChanges();

            // Add join table entries for assigned users
            foreach (var userId in assigneeUserIds.Distinct())
            {
                var sprintUser = new SprintUser
                {
                    SprintId = sprint.Id,
                    UserId = userId
                };
                _context.SprintUsers.Add(sprintUser);
            }

            _context.SaveChanges();
            return sprint;
        }

        public List<Sprint> GetAllSprints()
        {
            return _context.Sprints
                .Include(s => s.Project)
                .Include(s => s.CreatedByUser)
                .Include(s => s.SprintUsers)
                    .ThenInclude(su => su.User)
                .Include(s => s.Tasks)
                    .ThenInclude(t => t.WorkTaskLogs)
                .ToList();
        }

        public Sprint GetSprintById(int id)
        {
            return _context.Sprints.Include(s => s.Project).Include(s => s.CreatedByUser).Include(s => s.SprintUsers).ThenInclude(su => su.User).FirstOrDefault(s => s.Id == id);
        }
        public void UpdateSprint(int id, Sprint updatedSprint, List<int> assigneeUserIds)
        {
            var existing = _context.Sprints.Include(s => s.SprintUsers).FirstOrDefault(s => s.Id == id);
            if (existing == null) return;

            // Update fields
            existing.SprintName = updatedSprint.SprintName;
            existing.SprintGoal = updatedSprint.SprintGoal;
            existing.StartDate = updatedSprint.StartDate;
            existing.EndDate = updatedSprint.EndDate;
            existing.Status = updatedSprint.Status;

            // Update assignees
            _context.SprintUsers.RemoveRange(existing.SprintUsers);

            foreach (var userId in assigneeUserIds.Distinct())
            {
                _context.SprintUsers.Add(new SprintUser
                {
                    SprintId = existing.Id,
                    UserId = userId
                });
            }

            _context.SaveChanges();
        }

        public bool DeleteSprint(int id)
        {
            var sprint = _context.Sprints.FirstOrDefault(s => s.Id == id);
            if (sprint == null)
                return false;

            _context.Sprints.Remove(sprint);
            _context.SaveChanges();
            return true;
        }
        public User GetAssigneeById(int assigneeId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == assigneeId);
        }
        public Project GetProjectById(string projectId)
        {
            return _context.Projects.FirstOrDefault(p => p.Id == projectId);
        }
        public void AssignUsersToSprint(int sprintId, List<int> assigneeIds)
        {
            foreach (var userId in assigneeIds)
            {
                if (!_context.SprintUsers.Any(su => su.SprintId == sprintId && su.UserId == userId))
                {
                    _context.SprintUsers.Add(new SprintUser
                    {
                        SprintId = sprintId,
                        UserId = userId
                    });
                }
            }
            _context.SaveChanges();
        }
        public Sprint GetSprintWithMetricsById(int id)
        {
            return _context.Sprints
                .Include(s => s.SprintUsers)
                    .ThenInclude(su => su.User)
                .Include(s => s.Project)
                .Include(s => s.CreatedByUser)
                .FirstOrDefault(s => s.Id == id);
        }
        public List<WorkTask> GetTasksBySprintId(int sprintId)
        {
            return _context.Tasks
                .Where(t => t.SprintId == sprintId)
                .Include(t => t.WorkTaskUsers)
                .Include(t => t.WorkTaskLogs)
                .ToList();
        }
        public List<WorkTaskLog> GetLogsBySprintId(int sprintId)
        {
            var taskIds = _context.Tasks
                .Where(t => t.SprintId == sprintId)
                .Select(t => t.Id)
                .ToList();

            return _context.WorkTaskLogs
                .Where(log => taskIds.Contains(log.WorkTaskId))
                .ToList();
        }

    }
}
