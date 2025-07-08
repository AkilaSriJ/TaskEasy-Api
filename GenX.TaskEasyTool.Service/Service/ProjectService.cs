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
    public class ProjectService:IProjectService
    {
        private readonly IProjectRepository _repository;
        private readonly IProjectLogRepository _projectLogRepository;

        public ProjectService(IProjectRepository repository, IProjectLogRepository projectLogRepository)
        {
            _repository = repository;
            _projectLogRepository = projectLogRepository;
        }

        public ProjectResponseDto CreateProject(ProjectCreateDto dto)
        {
            var user = _repository.GetUserByUserName(dto.CreatedByUser);
            if (user == null)
                throw new Exception("User not found");

            var lastProject = _repository.GetLastProject();
            int lastNumber = 0;

            if (lastProject != null && !string.IsNullOrEmpty(lastProject.Id) && lastProject.Id.StartsWith("PROJ-"))
            {
                string numberPart = lastProject.Id.Substring(5);
                int.TryParse(numberPart, out lastNumber);
            }

            string newId = $"PROJ-{(lastNumber + 1):D3}";

            var project = new Project
            {
                Id = newId,
                ProjectName = dto.ProjectName,
                ProjectDescription = dto.ProjectDescription,
                CreatedByUserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ProjectUsers = new List<ProjectUser>()
            };

            var created = _repository.CreateProject(project);
            if (created == null)
                throw new Exception("Project creation failed");
            
            _projectLogRepository.Add(new ProjectLog
            {
                ProjectId = created.Id,
                Action = "Create",
                PerformedBy = user.Username,
                PerformedAt = DateTime.UtcNow,
                Description = $"Project '{created.ProjectName}' created"
            });

            if (dto.AssignedUserIds != null && dto.AssignedUserIds.Any())
            {
                _repository.AssignUsersToProject(project.Id, dto.AssignedUserIds);
            }

            var assignees = dto.AssignedUserIds?
                .Select(id =>
                {
                    var assignedUser = _repository.GetUserById(id);
                    return assignedUser != null
                        ? new AssigneeDto { Id = assignedUser.Id, Name = assignedUser.Username }
                        : null;
                })
                .Where(a => a != null)
                .ToList() ?? new List<AssigneeDto>();

            return new ProjectResponseDto
            {
                Id = created.Id,
                ProjectName = created.ProjectName,
                ProjectDescription = created.ProjectDescription,
                CreatedByUser = user.Username,
                Assignees = assignees
            };
        }

        public List<ProjectResponseDto> GetAllProjects()
        {
            var projects = _repository.GetAllWithUsers(); // Include Users
            return projects.Select(p => new ProjectResponseDto
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                ProjectDescription = p.ProjectDescription,
                CreatedByUser = p.CreatedByUser?.Username,
                Assignees = p.ProjectUsers?.Select(u => new AssigneeDto
                {
                    Id = u.User.Id,
                    Name = u.User.Username
                }).ToList()
            }).ToList();
        }

        public ProjectResponseDto GetProjectById(string id)
        {
            var project = _repository.GetByIdWithUsers(id);
            if (project == null) return null;

            return new ProjectResponseDto
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                ProjectDescription = project.ProjectDescription,
                CreatedByUser = project.CreatedByUser?.Username,
                Assignees = project.ProjectUsers?.Select(u => new AssigneeDto
                {
                    Id = u.User.Id,
                    Name = u.User.Username
                }).ToList()
            };
        }
       

        public void DeleteProject(string id)
        {
            var project = _repository.GetProjectById(id);
            if (project == null) return;

            _repository.Delete(project);
            _projectLogRepository.Add(new ProjectLog
            {
                ProjectId = project.Id,
                Action = "Delete",
                PerformedBy = "System",
                PerformedAt = DateTime.UtcNow,
                Description = $"Project '{project.ProjectName}' was deleted"
            });
        }
        public List<ProjectNameIdDto> GetAllProjectNamesAndIds()
        {
            var projects = _repository.GetAllProjects();

            return projects.Select(p => new ProjectNameIdDto
            {
                Id = p.Id,
                Name = p.ProjectName,
                ProjectDescription=p.ProjectDescription
            }).ToList();
        }


    }
}
