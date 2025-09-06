using Project.Application.Interfaces.IDataSeedingServices;
using Project.Application.Interfaces.IExternalServices;
using Project.Application.Interfaces.IExternalServices.ITokenServices;
using Project.Application.Interfaces.IServices;
using Project.Application.Services;
using Project.Domain.Interfaces.IRepositories;
using Project.Infrastructure.Data.DataSeedingServices;
using Project.Infrastructure.Data.Repositories;
using Project.Infrastructure.ExternalServices.StorageServices;
using Project.Infrastructure.ExternalServices.TokenServices;

namespace Project.API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection Register(this IServiceCollection services)
        {
            RegisterServices(services);
            RegisterRepositories(services);
            RegisterSeedData(services);
            return services;
        }
        public static IServiceCollection RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
        public static IServiceCollection RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
        public static IServiceCollection RegisterSeedData(IServiceCollection services)
        {
            services.AddScoped<IDataSeedingService, DataSeedingService>();
            return services;
        }
        public static IServiceCollection RegisterValidator(IServiceCollection services)
        {
            return services;
        }
    }
}
