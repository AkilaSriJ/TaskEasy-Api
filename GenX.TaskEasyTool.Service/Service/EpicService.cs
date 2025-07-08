using GenX.TaskEasyTool.Data.Interface;
using GenX.TaskEasyTool.Data.Models;
using GenX.TaskEasyTool.Data.Repository;
using GenX.TaskEasyTool.Model.DTO_S;
using GenX.TaskEasyTool.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenX.TaskEasyTool.Service.Service
{
    public class EpicService : IEpicService
    {
        private readonly IEpicRepository _repository;

        private readonly IEpicLogRepository _epicLogRepository;
        private readonly ILabelService _labelService;
        public EpicService(IEpicRepository repository, IEpicLogRepository epicLogRepository, ILabelService labelService)
        {
            _repository = repository;
            _epicLogRepository = epicLogRepository;
            _labelService = labelService;
        }

        public EpicResponseDto CreateEpic(EpicCreateDto dto)
        {
            var project = _repository.GetProjectById(dto.ProjectId);
            if (dto.ProjectId != null && project == null)
                throw new Exception("Project not found.");
            var label = _labelService.GetOrCreateLabel(dto.Label.Name);

            var epic = new Epic
            {
                ProjectId = dto.ProjectId,
                EpicName = dto.EpicName,
                SummaryOfTheEpic = dto.SummaryOfTheEpic,
                EpicWorkflow = dto.EpicWorkflow,
                LabelId = label.Id,
                CreatedAt = DateTime.UtcNow,
                EpicUsers = new List<EpicUser>()
            };

            var created = _repository.CreateEpic(epic);
            _epicLogRepository.Add(new EpicLog
            {
                EpicId = created.Id,
                Action = "Create",
                PerformedBy = dto.CreatedByUser, 
                PerformedAt = DateTime.UtcNow,
                Description = $"Epic '{created.EpicName}' created in project '{project?.ProjectName}'"
            });

           
            if (dto.AssigneeIds != null && dto.AssigneeIds.Any())
            {
                _repository.AssignUsersToEpic(created.Id, dto.AssigneeIds);
            }

           
            var assignees = dto.AssigneeIds
                .Select(id =>
                {
                    var user = _repository.GetAssigneeById(id);
                    return user != null ? new AssigneeDto { Id = user.Id, Name = user.Username } : null;
                })
                .Where(a => a != null)
                .ToList();

            return new EpicResponseDto
            {
                Id = created.Id,
                ProjectId = created.ProjectId,
                ProjectName = project?.ProjectName,
                EpicName = created.EpicName,
                SummaryOfTheEpic = created.SummaryOfTheEpic,
                EpicWorkflow = created.EpicWorkflow.ToString(),
                Assignees = assignees,
                Label = created.Label?.Name,
            };

        }
        public List<EpicResponseDto> GetAllEpics()
        {
            var epics = _repository.GetAllWithUsersAndProject();

            return epics.Select(e => new EpicResponseDto
            {
                Id = e.Id,
                ProjectId = e.ProjectId,
                ProjectName = e.Project?.ProjectName,
                EpicName = e.EpicName,
                SummaryOfTheEpic = e.SummaryOfTheEpic,
                EpicWorkflow = e.EpicWorkflow.ToString(),
                Label = e.Label?.Name,
                Assignees = e.EpicUsers?.Select(u => new AssigneeDto
                {
                    Id = u.User.Id,
                    Name = u.User.Username
                }).ToList()
            }).ToList();
        }

        public EpicResponseDto GetEpicById(int id)
        {
            var e = _repository.GetByIdWithUsersAndProject(id);
            if (e == null) return null;

            return new EpicResponseDto
            {
                Id = e.Id,
                ProjectId = e.ProjectId,
                ProjectName = e.Project?.ProjectName,
                EpicName = e.EpicName,
                SummaryOfTheEpic = e.SummaryOfTheEpic,
                EpicWorkflow = e.EpicWorkflow.ToString(),
                Label = e.Label?.Name,
                Assignees = e.EpicUsers?.Select(u => new AssigneeDto
                {
                    Id = u.User.Id,
                    Name = u.User.Username
                }).ToList()
            };
        }
        public void DeleteEpic(int id)
        {
            var epic = _repository.GetById(id);
            if (epic == null) return;

            _repository.Delete(epic);
        }

    }
}
