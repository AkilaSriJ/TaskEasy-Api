using GenX.TaskEasyTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Interface
{
    public interface IProjectLogRepository
    {
        void Add(ProjectLog log);
        List<ProjectLog> GetLogsByProjectId(string projectId);

    }
}
