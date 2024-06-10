using Microsoft.EntityFrameworkCore;
using Shortify.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortify.Data.Services
{
    public class UrlsService : IUrlsService
    {
        private readonly AppDbContext dbContext;

        public UrlsService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Url> AddAsync(Url url)
        {
            await dbContext.Urls.AddAsync(url);
            await dbContext.SaveChangesAsync();
            return url;
        }

        public async Task DeleteAsync(int id)
        {
            var dbUrl = await dbContext.Urls.FirstOrDefaultAsync(u => u.Id == id);
            if (dbUrl != null)
            {
                dbContext.Urls.Remove(dbUrl);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<Url> GetByIdAsync(int id)
        {
            var url = await dbContext.Urls.FirstOrDefaultAsync(u => u.Id == id);
            return url;
        }

        public async Task<IEnumerable<Url>> GetUrlsAsync(string userId, bool isAdmin)
        {
            var allUrlsQuery = dbContext.Urls.Include(u => u.User);

            if (isAdmin)
            {
                return await allUrlsQuery.ToListAsync();
            }
            else
            {
                return await allUrlsQuery.Where(n => n.UserId == userId).ToListAsync();
            }

        }

        public async Task<Url> UpdateAsync(int id, Url url)
        {
            var dbUrl = await dbContext.Urls.FirstOrDefaultAsync(u => u.Id == id);
            if(dbUrl != null)
            {
                dbUrl.OriginalLink = url.OriginalLink;
                dbUrl.ShortLink = url.ShortLink;
                dbUrl.DateUpdated = DateTime.UtcNow;

                await dbContext.SaveChangesAsync();
            }

            return dbUrl;
        }
    }
}
