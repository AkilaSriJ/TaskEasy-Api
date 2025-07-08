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
    public class WorkTaskLogRepository: IWorkTaskLogRepository
    {
        private readonly AppDbContext _context;

        public WorkTaskLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(WorkTaskLog log)
        {
            var userExists = _context.Users.Any(u => u.Id == log.UserId);
            var taskExists = _context.Tasks.Any(t => t.Id == log.WorkTaskId);

            if (!userExists)
            {
                Console.WriteLine($"UserId {log.UserId} does not exist.");
                return;
            }

            if (!taskExists)
            {
                Console.WriteLine($"WorkTaskId {log.WorkTaskId} does not exist.");
                return;
            }

            try
            {
                _context.WorkTaskLogs.Add(log);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("DB Update Exception: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                throw;
            }
        }

        public void Update(WorkTaskLog log)
        {
            _context.WorkTaskLogs.Update(log);
            _context.SaveChanges();
        }

        public List<WorkTaskLog> GetAll()
        {
            return _context.WorkTaskLogs.Include(l => l.WorkTask).ToList();
        }

        public WorkTaskLog GetById(int id)
        {
            return _context.WorkTaskLogs.AsTracking() .FirstOrDefault(log => log.Id == id);
        }
        public WorkTaskLog CreateLog(WorkTaskLog log)
        {
            _context.WorkTaskLogs.Add(log);
            _context.SaveChanges();
            return log;
        }

        public List<WorkTaskLog> GetLogsBySprint(int sprintId)
        {
            return _context.WorkTaskLogs
                .Include(x => x.User)
                .Include(x => x.WorkTask)
                .Where(x => x.WorkTask.SprintId == sprintId)
                .ToList();
        }
        public List<WorkTaskLog> GetLogsByTaskId(int taskId)
        {
            return _context.WorkTaskLogs
                .Include(l => l.User)
                .Include(l => l.WorkTask)
                .Where(l => l.WorkTaskId == taskId)
                .OrderByDescending(l => l.LoggedAt)
                .ToList();
        }

    }
}
