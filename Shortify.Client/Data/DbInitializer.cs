using Shortify.Data;
using Shortify.Data.Models;

namespace Shortify.Client.Data
{
    public static class DbInitializer
    {
        public static void SeedDefaultData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();

                if (dbContext != null && !dbContext.Users.Any())
                {
                    dbContext.Users.Add(
                        new User()
                        {
                            FullName = "Olegs Osipovs",
                            Email = "olegs@osipovs.com"
                        });

                    dbContext.SaveChanges();
                }

                if(dbContext != null & !dbContext.Urls.Any())
                {
                    dbContext.Urls.Add(new Url
                    {
                        OriginalLink = "https://dotnethow.net",
                        ShortLink = "dnh",
                        NrOfClicks = 20,
                        DateCreated = DateTime.Now,
                        UserId = dbContext.Users.First().Id
                    });

                    dbContext.SaveChanges();
                }
            }
        }
    }
}
