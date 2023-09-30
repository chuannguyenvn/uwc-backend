using Repositories.Managers;

namespace Helpers;

public static class WebApplicationExtensions
{
    public static WebApplication ResetData(this WebApplication webApplication)
    {
        var scopedFactory = webApplication.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopedFactory.CreateScope();
        using var dbContext = scope.ServiceProvider.GetService<UwcDbContext>();
        if (dbContext == null) throw new NullReferenceException("Database is not present for seeding.");

        var dataSeeder = new DatabaseSeeder(dbContext);
        dataSeeder.ResetDatabase();

        return webApplication;
    }

    public static WebApplication SeedData(this WebApplication webApplication)
    {
        var scopedFactory = webApplication.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopedFactory.CreateScope();
        using var dbContext = scope.ServiceProvider.GetService<UwcDbContext>();
        if (dbContext == null) throw new NullReferenceException("Database is not present for seeding.");

        var dataSeeder = new DatabaseSeeder(dbContext);

        dataSeeder.SeedDatabase();

        return webApplication;
    }
}