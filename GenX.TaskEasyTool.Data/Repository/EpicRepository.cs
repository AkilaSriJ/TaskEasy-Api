using GenX.TaskEasyTool.Data.Context;
using GenX.TaskEasyTool.Data.Interface;
using GenX.TaskEasyTool.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Repository
{
    public class EpicRepository:IEpicRepository
    {
        private readonly AppDbContext _context;

        public EpicRepository(AppDbContext context)
        {
            _context = context;
        }

        public Epic CreateEpic(Epic epic)
        {
            _context.Epics.Add(epic);
            _context.SaveChanges();
            return epic;
        }
        public void AssignUsersToEpic(int epicId, List<int> assigneeIds)
        {
            foreach (var userId in assigneeIds)
            {
                if (!_context.EpicUsers.Any(eu => eu.EpicId == epicId && eu.UserId == userId))
                {
                    _context.EpicUsers.Add(new EpicUser
                    {
                        EpicId = epicId,
                        UserId = userId
                    });
                }
            }
            _context.SaveChanges();
        }

        public Project GetProjectById(string projectId)
        {
            return _context.Projects.FirstOrDefault(p => p.Id == projectId);
        }

        public User GetAssigneeById(int assigneeId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == assigneeId);
        }
        public List<Epic> GetAllWithUsersAndProject()
        {
            return _context.Epics
                .Include(e => e.Label)
                .Include(e => e.Project)
                .Include(e => e.EpicUsers)
                    .ThenInclude(eu => eu.User)
                .ToList();
        }

        public Epic GetByIdWithUsersAndProject(int id)
        {
            return _context.Epics
                .Include(e => e.Label)
                .Include(e => e.Project)
                .Include(e => e.EpicUsers)
                    .ThenInclude(eu => eu.User)
                .FirstOrDefault(e => e.Id == id);
        }
        public Epic GetById(int id)
        {
            return _context.Epics.FirstOrDefault(e => e.Id == id);
        }

        public void Delete(Epic epic)
        {
            _context.Epics.Remove(epic);
            _context.SaveChanges();
        }

    }
}
