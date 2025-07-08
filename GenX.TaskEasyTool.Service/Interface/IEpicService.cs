using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Model.DTO_S;

namespace GenX.TaskEasyTool.Service.Interface
{
    public interface IEpicService
    {
        EpicResponseDto CreateEpic(EpicCreateDto dto);
        List<EpicResponseDto> GetAllEpics();
        EpicResponseDto GetEpicById(int id);
        void DeleteEpic(int id);


    }
}
