using GenX.TaskEasyTool.Data.Context;
using GenX.TaskEasyTool.Data.Interface;
using GenX.TaskEasyTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Repository
{
    public class SprintLogRepository:ISprintLogRepository
    {
        private readonly AppDbContext _context;

        public SprintLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(SprintLog log)
        {
            _context.SprintLogs.Add(log);
            _context.SaveChanges();
        }
        public List<SprintLog> GetLogsBySprintId(int sprintId)
        {
            return _context.SprintLogs
                .Where(log => log.SprintId == sprintId)
                .OrderByDescending(log => log.PerformedAt)
                .ToList();
        }

    }
}
