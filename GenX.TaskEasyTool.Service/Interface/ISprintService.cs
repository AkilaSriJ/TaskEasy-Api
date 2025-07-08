using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Model.DTO_S;
using GenX.TaskEasyTool.Model;

namespace GenX.TaskEasyTool.Service.Interface
{
    public interface ISprintService
    {
        ResponseSprintDto CreateSprint(SprintCreateDto dto);
        List<ResponseSprintDto> GetAllSprints();
        ResponseSprintDto GetSprintById(int id);
        void UpdateSprint(int id, SprintCreateDto dto);
        bool DeleteSprint(int id);
        ResponseSprintDto GetSprintOverviewById(int id);

    }
}
