using JWTServer.Processor.Configuration;
using JWTServer.Processor;
using JWTServer.ServiceRepository;
using JWTServer.ViewModels.Users;

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
        public static void ConfigureProcessor(this IServiceCollection services)
        {
            #region Configuration
            ConfigureConfigurationProcessor(services);
            #endregion
        }
        private static void ConfigureConfigurationProcessor(IServiceCollection services)
        {
            //services.AddScoped<IProcessor<CompanyBaseModel>, CompanyProcessor>();
            //services.AddScoped<IProcessor<BranchBaseModel>, BranchProcessor>();
            //services.AddScoped<IProcessor<MenuModuleBaseModel>, MenuModuleProcessor>();
            //services.AddScoped<IProcessor<MenuCategoryBaseModel>, MenuCategoryProcessor>();
            //services.AddScoped<IProcessor<MenuSubCategoryBaseModel>, MenuSubCategoryProcessor>();
            //services.AddScoped<IProcessor<UserRoleBaseModel>, UserRoleProcessor>();
            services.AddScoped<IProcessor<UserLoginBaseModel>, UserProcessor>();
        }
    }
}
