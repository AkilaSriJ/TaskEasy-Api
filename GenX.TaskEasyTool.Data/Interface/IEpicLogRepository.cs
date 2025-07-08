using GenX.TaskEasyTool.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Data.Interface
{
    public interface IEpicLogRepository
    {
        void Add(EpicLog log);
        List<EpicLog> GetLogsByEpicId(int epicId);


    }
}
