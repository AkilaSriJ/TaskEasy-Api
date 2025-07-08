using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Model.DTO_S;

namespace GenX.TaskEasyTool.Service.Interface
{
    public interface IProjectService
    {
        ProjectResponseDto CreateProject(ProjectCreateDto dto);
        List<ProjectResponseDto> GetAllProjects();
        ProjectResponseDto GetProjectById(string id);
       
        void DeleteProject(string id);
        List<ProjectNameIdDto> GetAllProjectNamesAndIds();


    }
}
