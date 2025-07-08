using GenX.TaskEasyTool.Data.Interface;
using GenX.TaskEasyTool.Data.Repository;
using GenX.TaskEasyTool.Service.Interface;
using GenX.TaskEasyTool.Service.Service;


namespace GenX.TaskEasyTool.Api.DependencyInjection
{
    public static class ServiceDI
    {
        public static IServiceCollection AddServiceDI(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IEpicService, EpicService>();
            services.AddScoped<ISprintService, SprintService>();
            services.AddScoped<ILabelService, LabelService>();
            services.AddScoped<IWorkTaskService, WorkTaskService>();

            return services;
        }
    }
}
