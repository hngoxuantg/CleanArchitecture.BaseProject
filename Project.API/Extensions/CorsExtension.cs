﻿namespace Project.API.Extensions
{
    public static class CorsExtension
    {
        private const string CustomCorsPolycy = "CustomCorsPolicy";
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
        {
            string[]? origins = configuration.GetSection("AllowedCors:Origins").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy(CustomCorsPolycy, policy =>
                {
                    if (origins != null)
                    {
                        policy.WithOrigins(origins)
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    }
                    else
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    }
                });
            });

            return services;
        }
        public static string GetPolicyName() => CustomCorsPolycy;
    }
}
