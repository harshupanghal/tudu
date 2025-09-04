// Infrastructure/DependencyInjection.cs
using Application.Interfaces;
using Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;
using Tudu.Application.Interfaces;
using Tudu.Infrastructure.Services;

namespace Infrastructure
    {
    public static class DependencyInjection
        {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
            {
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddTransient<IEmailService, SendGridEmailService>();
            return services;
            }
        }
    }
