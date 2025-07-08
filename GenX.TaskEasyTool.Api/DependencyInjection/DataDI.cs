using GenX.TaskEasyTool.Data.Interface;
using GenX.TaskEasyTool.Data.Repository;


namespace GenX.TaskEasyTool.Api.DependencyInjection
{
    public static class DataDI
    {
        public static IServiceCollection AddDataDI(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IEpicRepository, EpicRepository>();
            services.AddScoped<ISprintRepository, SprintRepository>();
            services.AddScoped<ILabelRepository, LabelRepository>();
            services.AddScoped<IWorkTaskRepository, WorkTaskRepository>();
            services.AddScoped<IWorkTaskLogRepository, WorkTaskLogRepository>();
            services.AddScoped<IProjectLogRepository, ProjectLogRepository>();
            services.AddScoped<ISprintLogRepository, SprintLogRepository>();
            services.AddScoped<IEpicLogRepository, EpicLogRepository>();
            return services;
        }
    }
}
