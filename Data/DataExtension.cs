using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using server.Repositories;

namespace server.Data
{
    public static class DataExtension
    {
        public static async Task InitializeDataBaseAsync(this  IServiceProvider serviceProvider)
        {
            using var scope  = serviceProvider.CreateScope();
            var dbContext  = scope.ServiceProvider.GetRequiredService<FinanceContext>();
             await dbContext.Database.MigrateAsync();
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("FinanceContext");
            services.AddSqlServer<FinanceContext>(connString)
                    .AddScoped<IUserRepository, UserRepository>()
                    .AddScoped<IExpenseRepository, ExpenseRepository>()
                    .AddTransient<IPasswordHasher, PasswordHasher>()
                    .AddTransient<IHttpContextAccessor, HttpContextAccessor>();

             return services;       
        }
        
    }
}