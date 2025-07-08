using GenX.TaskEasyTool.Data.Models;
using GenX.TaskEasyTool.Model.DTO_S;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Service.Interface
{
    public interface IWorkTaskService
    {
        WorkTaskResponseDto CreateWorkTask(WorkTaskCreateDto dto);
        WorkTaskResponseDto ToResponseDto(WorkTask task);
        List<User> GetAssigneeUsers(int taskId);
        List<WorkTaskResponseDto> GetAllWorkTasks();
        WorkTaskResponseDto GetWorkTaskById(int id);

        void AddWorkTaskLog(WorkTaskLogCreateDto dto);
        void UpdateWorkTaskLog(WorkTaskLogUpdateDto dto);
        List<WorkTaskLogResponseDto> GetAllLogs();
        WorkTaskLogResponseDto GetLogById(int id);
        WorkTaskResponseDto ReplaceAssignees(WorkTaskAssigneeUpdateDto dto);
        List<WorkTaskResponseDto> GetProductBacklog(string projectId);
        List<WorkTaskResponseDto> GetAssignedTasksInSprint(int sprintId);

       
        void DeleteWorkTask(int taskId);

    }
}
