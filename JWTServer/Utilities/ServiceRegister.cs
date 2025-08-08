using JWTServer.ServiceRepository;

namespace JWTServer.Utilities
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register your app services here
            services.AddScoped<IAuthServiceRepository, AuthServiceRepository>();

            // You can add more as needed later
            return services;
        }
    }
}
