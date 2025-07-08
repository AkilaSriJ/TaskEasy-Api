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
    public class EpicLogRepository:IEpicLogRepository
    {
        private readonly AppDbContext _context;

        public EpicLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(EpicLog log)
        {
            _context.EpicLogs.Add(log);
            _context.SaveChanges();
        }
        public List<EpicLog> GetLogsByEpicId(int epicId)
        {
            return _context.EpicLogs
                .Where(log => log.EpicId == epicId)
                .OrderByDescending(log => log.PerformedAt)
                .ToList();
        }

    }
}
