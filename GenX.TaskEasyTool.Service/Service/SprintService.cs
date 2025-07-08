using GenX.TaskEasyTool.Common.Enum;
using GenX.TaskEasyTool.Data.Interface;
using GenX.TaskEasyTool.Data.Models;
using GenX.TaskEasyTool.Data.Repository;
using GenX.TaskEasyTool.Model.DTO_S;
using GenX.TaskEasyTool.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Service.Service
{
    public class SprintService : ISprintService
    {
        private readonly ISprintRepository _repository;
        private readonly ISprintLogRepository _sprintLogRepository;

        public SprintService(ISprintRepository repository, ISprintLogRepository sprintLogRepository)
        {
            _repository = repository;
            _sprintLogRepository = sprintLogRepository;
        }

        public ResponseSprintDto CreateSprint(SprintCreateDto dto)
        {
            var project = _repository.GetProjectById(dto.ProjectId);
            if (dto.ProjectId != null && project == null)
                throw new Exception("Project not found.");

            var sprint = new Sprint
            {
                SprintName = dto.SprintName,
                SprintGoal = dto.SprintGoal,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status,
                ProjectId = dto.ProjectId,
                CreatedByUserId = dto.CreatedByUserId,
                CreatedAt = DateTime.UtcNow,
                SprintUsers = new List<SprintUser>()
            };

            var created = _repository.CreateSprint(sprint,dto.AssigneeUserIds);
            
            _sprintLogRepository.Add(new SprintLog
            {
                SprintId = created.Id,
                Action = "Create",
                PerformedBy = _repository.GetAssigneeById(dto.CreatedByUserId)?.Username ?? "Unknown",
                PerformedAt = DateTime.UtcNow,
                Description = $"Sprint '{created.SprintName}' created"
            });

            if (dto.AssigneeUserIds != null && dto.AssigneeUserIds.Any())
            {
                _repository.AssignUsersToSprint(created.Id, dto.AssigneeUserIds);
            }
            var fullSprint = _repository.GetSprintWithMetricsById(created.Id);

            var tasks = _repository.GetTasksBySprintId(created.Id);
            var logs = _repository.GetLogsBySprintId(created.Id);

            var userActiveTasks = fullSprint.SprintUsers.Select(su =>
            {
                var userId = su.UserId;
                var userName = su.User.Username;

                var activeTaskCount = tasks
                    .Where(t => t.Status == Status.InProgress)
                    .Count(t => t.WorkTaskUsers.Any(wtu => wtu.UserId == userId));

                return new UserActiveTaskDto
                {
                    UserId = userId,
                    UserName = userName,
                    ActiveTaskCount = activeTaskCount
                };
            }).ToList();

            var assignees = dto.AssigneeUserIds
                .Select(id =>
                {
                    var user = _repository.GetAssigneeById(id);
                    return user != null ? new AssigneeDto { Id = user.Id, Name = user.Username } : null;
                })
                .Where(a => a != null)
                .ToList();

            return new ResponseSprintDto
            {
                Id = created.Id,
                SprintName = created.SprintName,
                SprintGoal = created.SprintGoal,
                StartDate = created.StartDate,
                EndDate = created.EndDate,
                Status = created.Status.ToString(),
                ProjectId = created.ProjectId,
                ProjectName = project?.ProjectName,
                CreatedByUserId = created.CreatedByUserId,
                CreatedByUserName = _repository.GetAssigneeById(created.CreatedByUserId)?.Username,
                Assignees = assignees,
                TeamMembersCount = fullSprint.SprintUsers.Count,
                ActiveTaskCount = tasks.Count(t => t.Status == Status.InProgress),
                CompletedTaskCount = tasks.Count(t => t.Status == Status.Done),
                //HighPriorityTasks = tasks.Count(t => t.Priority == Priority.High),
                TotalHoursLogged = logs.Sum(l => l.HoursWorked),
                UserActiveTasks = userActiveTasks
            };
        }

        public List<ResponseSprintDto> GetAllSprints()
        {
            var sprints = _repository.GetAllSprints(); 

            return sprints.Select(s =>
            {
                var tasks = s.Tasks ?? new List<WorkTask>();
                var totalTasks = s.Tasks?.Count() ?? 0;
                var completedTasks = s.Tasks?.Count(t => t.Status == Status.Done) ?? 0;
                //var activeTasks = s.Tasks?.Count(t => t.Status == Status.CodeReview) ?? 0;
                //var highPriority = s.Tasks?.Count(t => t.Priority == Priority.High) ?? 0;
                var totalHours = s.Tasks?.SelectMany(t => t.WorkTaskLogs ?? new List<WorkTaskLog>()).Sum(log => log.HoursWorked) ?? 0;

                var totalSprintDays = (s.EndDate - s.StartDate).Days;
                var daysPassed = (DateTime.Now - s.StartDate).Days;
                var daysRemaining = (s.EndDate - DateTime.Now).Days;
                if (daysRemaining < 0) daysRemaining = 0;
                var sprintUsers = s.SprintUsers ?? new List<SprintUser>();

                var userActiveTasks = sprintUsers.Select(su =>
                {
                    var userId = su.UserId;
                    var userName = su.User.Username;

                    var activeTaskCount = (tasks ?? new List<WorkTask>())
                        .Where(t => t.Status == Status.InProgress)
                        .Count(t => t.WorkTaskUsers != null && t.WorkTaskUsers.Any(wtu => wtu.UserId == userId));


                    return new UserActiveTaskDto
                    {
                        UserId = userId,
                        UserName = userName,
                        ActiveTaskCount = activeTaskCount
                    };
                }).ToList();

                return new ResponseSprintDto
                {
                    Id = s.Id,
                    SprintName = s.SprintName,
                    SprintGoal = s.SprintGoal,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Status = s.Status.ToString(),
                    ProjectId = s.ProjectId,
                    ProjectName = s.Project?.ProjectName,
                    CreatedByUserId = s.CreatedByUserId,
                    CreatedByUserName = s.CreatedByUser?.Username,
                    Assignees = s.SprintUsers?.Select(su => new AssigneeDto
                    {
                        Id = su.User.Id,
                        Name = su.User.Username
                    }).ToList(),
                    UserActiveTasks = userActiveTasks,
                    TeamMembersCount = s.SprintUsers?.Count() ?? 0,
                    //ActiveTaskCount = activeTasks,
                    TotalTasks= totalTasks,
                    CompletedTaskCount = completedTasks,
                    //HighPriorityTasks = highPriority,
                    TotalHoursLogged = totalHours,
                   
                };
            }).ToList();
        }


        public ResponseSprintDto GetSprintById(int id)
        {
            var sprint = _repository.GetSprintById(id);
            return sprint != null ? MapToResponseDto(sprint) : null;
        }

        private ResponseSprintDto MapToResponseDto(Sprint sprint)
        {
            var tasks = _repository.GetTasksBySprintId(sprint.Id);
            var logs = _repository.GetLogsBySprintId(sprint.Id);

            int totalTasks = tasks.Count;
            int completedTasks = tasks.Count(t => t.Status == Status.Done);
            int activeTasks = tasks.Count(t => t.Status == Status.InProgress);
            //int highPriorityTasks = tasks.Count(t => t.Priority == Priority.High);
            int totalHoursLogged = logs.Sum(l => l.HoursWorked);

            int totalDays = (sprint.EndDate - sprint.StartDate).Days + 1;
            int daysPassed = (System.DateTime.UtcNow.Date - sprint.StartDate.Date).Days;
            int daysRemaining = (sprint.EndDate.Date - System.DateTime.UtcNow.Date).Days;
            if (daysPassed < 0) daysPassed = 0;
            if (daysRemaining < 0) daysRemaining = 0;

            int progress = totalDays == 0 ? 0 : (int)Math.Round((double)daysPassed / totalDays * 100);

            var userActiveTasks = sprint.SprintUsers.Select(su =>
            {
                int userId = su.UserId;
                string userName = su.User.Username;

                int activeTaskCount = tasks
                    .Where(t => t.Status == Status.InProgress)
                    .Count(t => t.WorkTaskUsers.Any(wtu => wtu.UserId == userId));

                return new UserActiveTaskDto
                {
                    UserId = userId,
                    UserName = userName,
                    ActiveTaskCount = activeTaskCount
                };
            }).ToList();

            return new ResponseSprintDto
            {
                Id = sprint.Id,
                SprintName = sprint.SprintName,
                SprintGoal = sprint.SprintGoal,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                Status = sprint.Status.ToString(),
                ProjectId = sprint.Project.Id,
                ProjectName = sprint.Project.ProjectName,
                CreatedByUserId = sprint.CreatedByUserId,
                CreatedByUserName = sprint.CreatedByUser.Username,
                Assignees = sprint.SprintUsers.Select(su => new AssigneeDto
                {
                    Id = su.UserId,
                    Name = su.User.Username
                }).ToList(),

                UserActiveTasks = userActiveTasks,
                TeamMembersCount = sprint.SprintUsers.Count,
                ActiveTaskCount = activeTasks,
                TotalTasks = totalTasks,
                CompletedTaskCount = completedTasks,
                //HighPriorityTasks = highPriorityTasks,
                TotalHoursLogged = totalHoursLogged
            };
        }


        public void UpdateSprint(int id, SprintCreateDto dto)
        {
            var updated = new Sprint
            {
                SprintName = dto.SprintName,
                SprintGoal = dto.SprintGoal,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status,
                ProjectId = dto.ProjectId,
                CreatedByUserId = dto.CreatedByUserId,
                CreatedAt = DateTime.UtcNow
            };

            _repository.UpdateSprint(id, updated, dto.AssigneeUserIds);
            _sprintLogRepository.Add(new SprintLog
            {
                SprintId = id,
                Action = "Update",
                PerformedBy = _repository.GetAssigneeById(dto.CreatedByUserId)?.Username ?? "Unknown",
                PerformedAt = DateTime.UtcNow,
                Description = $"Sprint '{dto.SprintName}' updated"
            });
        }

        public bool DeleteSprint(int id)
        {
            var sprint = _repository.GetSprintById(id);
            if (sprint == null) return false;

            var result = _repository.DeleteSprint(id);

            if (result)
            {
                _sprintLogRepository.Add(new SprintLog
                {
                    SprintId = sprint.Id,
                    Action = "Delete",
                    PerformedBy = sprint.CreatedByUser?.Username ?? "System",
                    PerformedAt = DateTime.UtcNow,
                    Description = $"Sprint '{sprint.SprintName}' deleted"
                });
            }

            return result;
        }
        public ResponseSprintDto GetSprintOverviewById(int id)
        {
            var sprint = _repository.GetSprintWithMetricsById(id);
            if (sprint == null) return null;

            var dto = new ResponseSprintDto
            {
                Id = sprint.Id,
                SprintName = sprint.SprintName,
                SprintGoal = sprint.SprintGoal,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                Status = sprint.Status.ToString(),
                ProjectId = sprint.ProjectId,
                ProjectName = sprint.Project?.ProjectName,
                CreatedByUserId = sprint.CreatedByUserId,
                CreatedByUserName = sprint.CreatedByUser?.Username,
                Assignees = sprint.SprintUsers.Select(su => new AssigneeDto
                {
                    Id = su.User.Id,
                    Name = su.User.Username,
                }).ToList()
            };

            // Metrics (Example: count tasks, hours, etc.)
            var tasks = _repository.GetTasksBySprintId(sprint.Id);

            var logs = _repository.GetLogsBySprintId(sprint.Id);

            // user active task status
            var userActiveTasks = sprint.SprintUsers.Select(su =>
            {
                var userId = su.UserId;
                var userName = su.User.Username;

                var activeTaskCount = tasks
                    .Where(t => t.Status == Status.InProgress)  // Use Status.Active if needed
                    .Count(t => t.WorkTaskUsers.Any(wtu => wtu.UserId == userId));

                return new UserActiveTaskDto
                {
                    UserId = userId,
                    UserName = userName,
                    ActiveTaskCount = activeTaskCount
                };
            }).ToList();


            dto.TeamMembersCount = sprint.SprintUsers.Count;
            dto.ActiveTaskCount = tasks.Count(t => t.Status == Status.InProgress);
            dto.CompletedTaskCount = tasks.Count(t => t.Status == Status.Done);
            //dto.HighPriorityTasks = tasks.Count(t => t.Priority == Priority.High);
            dto.TotalHoursLogged = logs.Sum(l => l.HoursWorked);
            dto.UserActiveTasks = userActiveTasks;

            return dto;
        }

    }
}
