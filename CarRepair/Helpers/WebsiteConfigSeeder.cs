using DataAccess;
using Model.DomainModel;

public static class WebsiteConfigSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<ApplicationDbContext>();

        if (!db.WebsiteConfigs.Any())
        {
            db.WebsiteConfigs.Add(new WebsiteConfig
            {
                PortalHomeTitle = "Fast & Transparent Car Repair Estimates",
                PortalHomeText = "Enter your car details, describe the issue, upload photos, and receive a mechanic-verified cost estimate before booking your appointment."
            });
            await db.SaveChangesAsync();
        }
    }
}
