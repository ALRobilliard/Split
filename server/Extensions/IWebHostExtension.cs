using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Split.Extensions
{
    public static class IWebHostExtension
    {
      public static IWebHost MigrateDatabase<T>(this IWebHost webHost) where T : DbContext
      {
        using (var scope = webHost.Services.CreateScope())
        {
          var services = scope.ServiceProvider;
          try
          {
            var db = services.GetRequiredService<T>();
            db.Database.Migrate();
          }
          catch (Exception ex)
          {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
          }
        }
        return webHost;
      }
    }
}