using GenX.TaskEasyTool.Data.Interface;
using GenX.TaskEasyTool.Data.Models;
using GenX.TaskEasyTool.Data.Repository;
using GenX.TaskEasyTool.Model.DTO_S;
using GenX.TaskEasyTool.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Service.Service
{
    public class WorkTaskService: IWorkTaskService
    {
        private readonly IWorkTaskRepository _taskRepo;
        private readonly IWorkTaskLogRepository _logRepo;
        private readonly ILabelService _labelService;

        public WorkTaskService(IWorkTaskRepository taskRepo, IWorkTaskLogRepository logRepo, ILabelService labelService)
        {
            _taskRepo = taskRepo;
            _logRepo = logRepo;
            _labelService = labelService;
        }

        public WorkTaskResponseDto CreateWorkTask(WorkTaskCreateDto dto)
        {
            try
            {
                var reporterUser = _taskRepo.GetUserById(dto.CreatedByUserId);
                if (reporterUser == null)
                    throw new Exception("Reporter user not found");

                var task = new WorkTask
                {
                    Summary = dto.Summary,
                    Description = dto.Description,
                    Type = dto.Type,
                    Status = dto.Status,
                    StoryPoints = dto.StoryPoints,
                    EstimatedHours = dto.EstimatedHours,
                    CompletedHours = 0,
                    RemainingHours = dto.EstimatedHours,
                    ProjectId = dto.ProjectId,
                    EpicId = dto.EpicId,
                    SprintId = dto.SprintId,
                    StartDate = dto.StartDate,
                    DueDate = dto.DueDate,
                    Reporter = reporterUser.Username,
                    CreatedAt = DateTime.UtcNow,
                    WorkTaskLabels = new List<WorkTaskLabel>()
                };

                foreach (var labelName in dto.LabelNames)
                {
                    var label = _labelService.GetOrCreateLabel(labelName);
                    task.WorkTaskLabels.Add(new WorkTaskLabel
                    {
                        LabelId = label.Id
                    });
                }

                var result = _taskRepo.Create(task, dto.AssigneeUserIds);

                if (result != null)
                {
                    var assigneeUsers = _taskRepo.GetAssigneeUsers(result.Id);
                    string assigneeNames = assigneeUsers != null && assigneeUsers.Any()
                        ? string.Join(", ", assigneeUsers.Select(u => u.Username))
                        : "No assignees";

                    var log = new WorkTaskLog
                    {
                        WorkTaskId = result.Id,
                        UserId = dto.CreatedByUserId,
                        StatusUpdate = "Created",
                        Comment = $"Task '{result.Summary}' created and assigned to: {assigneeNames}",
                        HoursWorked = 0,
                        LoggedAt = DateTime.UtcNow
                    };

                    _logRepo.Add(log);

                
                    return ToResponseDto(result);
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }



        public WorkTaskResponseDto ToResponseDto(WorkTask task)
        {
            var users = _taskRepo.GetAssigneeUsers(task.Id);

            var assignees = users.Select(u => new AssigneeDto
            {
                Id = u.Id,
                Name = u.Username
            }).ToList();

            return new WorkTaskResponseDto
            {
                Id = task.Id,
                Summary = task.Summary,
                Description = task.Description,
                Type = task.Type.ToString(),
                LabelNames = task.WorkTaskLabels != null ? task.WorkTaskLabels.Select(wtl => wtl.Label.Name).ToList():new List<string>(),
                Status = task.Status.ToString(),
                StoryPoints = task.StoryPoints,
                EstimatedHours = task.EstimatedHours,
                CompletedHours = task.CompletedHours,
                RemainingHours = task.RemainingHours,
                ProjectId = task.ProjectId,
                EpicId = task.EpicId,
                SprintId = task.SprintId,
                Assignees = assignees,
                Reporter = task.Reporter,
                StartDate = task.StartDate,
                DueDate = task.DueDate
            };
        }
        public List<User> GetAssigneeUsers(int taskId)
        {
            return _taskRepo.GetAssigneeUsers(taskId); 
        }
        public List<WorkTaskResponseDto> GetAllWorkTasks()
        {
            var tasks = _taskRepo.GetAll();
            return tasks.Select(t => ToResponseDto(t)).ToList();
        }

        public WorkTaskResponseDto GetWorkTaskById(int id)
        {
            var task = _taskRepo.GetById(id);
            if (task == null) return null;

            return ToResponseDto(task);
        }
        public void AddWorkTaskLog(WorkTaskLogCreateDto dto)
        {
            var task = _taskRepo.GetById(dto.WorkTaskId);
            if (task == null) return;

            var log = new WorkTaskLog
            {
                WorkTaskId = dto.WorkTaskId,
                UserId = dto.UserId,
                StatusUpdate = dto.StatusUpdate,
                HoursWorked = dto.HoursWorked,
                Comment = dto.Comment
            };

            _logRepo.Add(log);

            task.CompletedHours += dto.HoursWorked;
            task.RemainingHours = task.EstimatedHours - task.CompletedHours;

            _taskRepo.Update(task);
        }
        public void UpdateWorkTaskLog(WorkTaskLogUpdateDto dto)
        {
            var log = _logRepo.GetById(dto.Id);
            if (log == null) return;

            var task = _taskRepo.GetById(log.WorkTaskId);
            if (task == null) return;

            int oldHours = log.HoursWorked;
            int newHours = dto.HoursWorked;
            int delta = newHours - oldHours;

            task.CompletedHours += delta;
            task.RemainingHours = task.EstimatedHours - task.CompletedHours;

            log.StatusUpdate = dto.StatusUpdate;
            log.HoursWorked = newHours;
            log.Comment = dto.Comment;

            _taskRepo.Update(task);
            _logRepo.Update(log);
        }

        public List<WorkTaskLogResponseDto> GetAllLogs()
        {
            var logs = _logRepo.GetAll(); 
            return logs.Select(log => new WorkTaskLogResponseDto
            {
                Id = log.Id,
                WorkTaskId = log.WorkTaskId,
                StatusUpdate = log.StatusUpdate,
                HoursWorked = log.HoursWorked,
                Comment = log.Comment,
                TaskTitle = log.WorkTask?.Summary
            }).ToList();
        }

        public WorkTaskLogResponseDto GetLogById(int id)
        {
            var log = _logRepo.GetById(id);
            if (log == null) return null;

            return new WorkTaskLogResponseDto
            {
                Id = log.Id,
                WorkTaskId = log.WorkTaskId,
                StatusUpdate = log.StatusUpdate,
                HoursWorked = log.HoursWorked,
                Comment = log.Comment,
                TaskTitle = log.WorkTask?.Summary
            };
        }
        public WorkTaskResponseDto ReplaceAssignees(WorkTaskAssigneeUpdateDto dto)
        {
            var task = _taskRepo.GetTaskWithUsers(dto.TaskId);
            if (task == null) return null;

           
            task.WorkTaskUsers.Clear();

          
            foreach (var userId in dto.AssigneeIds.Distinct())
            {
                task.WorkTaskUsers.Add(new WorkTaskUser
                {
                    WorkTaskId = dto.TaskId,
                    UserId = userId
                });
            }
            _taskRepo.SaveChanges();

            task = _taskRepo.GetTaskWithUsers(dto.TaskId);

            return new WorkTaskResponseDto
            {
                Id = task.Id,
                Summary = task.Summary,
                Description = task.Description,
                Type = task.Type.ToString(),
                LabelNames = task.WorkTaskLabels != null ? task.WorkTaskLabels.Select(wtl => wtl.Label.Name).ToList() : new List<string>(),
                Status = task.Status.ToString(),
                StoryPoints = task.StoryPoints,
                EstimatedHours = task.EstimatedHours,
                CompletedHours = task.CompletedHours,
                RemainingHours = task.RemainingHours,
                ProjectId = task.ProjectId,
                EpicId = task.EpicId,
                SprintId = task.SprintId,
                Assignees = task.WorkTaskUsers
                 .Where(wtu => wtu.User != null)
                    .Select(wtu => new AssigneeDto
                    {
                        Id = wtu.User.Id,
                        Name = wtu.User.Username
                    }).ToList()
                };
        }
        public List<WorkTaskResponseDto> GetProductBacklog(string projectId)
        {
            var tasks = _taskRepo.GetProductBacklog(projectId);
            return tasks.Select(t => ToResponseDto(t)).ToList(); 
        }


        public List<WorkTaskResponseDto> GetAssignedTasksInSprint(int sprintId)
        {
            var tasks = _taskRepo.GetAssignedTasksInSprint(sprintId);
            return tasks.Select(t => ToResponseDto(t)).ToList();
        }

        public void DeleteWorkTask(int taskId)
        {
            _taskRepo.Delete(taskId);
        }


    }
}
