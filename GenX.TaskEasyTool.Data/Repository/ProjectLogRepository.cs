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
    public class ProjectLogRepository:IProjectLogRepository
    {
        private readonly AppDbContext _context;

        public ProjectLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(ProjectLog log)
        {
            try
            {
                _context.ProjectLogs.Add(log);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public List<ProjectLog> GetLogsByProjectId(string projectId)
        {
            return _context.ProjectLogs
                .Where(log => log.ProjectId == projectId)
                .OrderByDescending(log => log.PerformedAt)
                .ToList();
        }
    }
}
