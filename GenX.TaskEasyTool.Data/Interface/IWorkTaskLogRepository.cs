using GenX.TaskEasyTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Interface
{
    public interface IWorkTaskLogRepository
    {
        void Add(WorkTaskLog log);
        void Update(WorkTaskLog log);
        List<WorkTaskLog> GetAll();
        WorkTaskLog GetById(int id);
        WorkTaskLog CreateLog(WorkTaskLog log);
        List<WorkTaskLog> GetLogsBySprint(int sprintId);
        List<WorkTaskLog> GetLogsByTaskId(int taskId);


    }
}
