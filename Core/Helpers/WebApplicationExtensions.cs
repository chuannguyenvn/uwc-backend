using Repositories.Managers;

namespace Helpers;

public static class WebApplicationExtensions
{
    public static WebApplication SeedData(this WebApplication webApplication)
    {
        var scopedFactory = webApplication.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopedFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
        if (unitOfWork == null) throw new NullReferenceException("Database is not present for seeding.");

        var dataSeeder = new DatabaseSeeder(unitOfWork);

        dataSeeder.SeedDatabase();

        return webApplication;
    }
}