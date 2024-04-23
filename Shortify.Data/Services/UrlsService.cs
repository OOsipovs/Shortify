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

        public Url Add(Url url)
        {
            dbContext.Urls.Add(url);
            dbContext.SaveChanges();
            return url;
        }

        public void Delete(int id)
        {
            var dbUrl = dbContext.Urls.FirstOrDefault(u => u.Id == id);
            if (dbUrl != null)
            {
                dbContext.Urls.Remove(dbUrl);
                dbContext.SaveChanges();
            }
        }

        public Url GetById(int id)
        {
            var url = dbContext.Urls.FirstOrDefault(u => u.Id == id);
            return url;
        }

        public IEnumerable<Url> GetUrls()
        {
            var allUrls = dbContext.Urls.Include(u => u.User).ToList();
            return allUrls;
        }

        public Url Update(int id, Url url)
        {
            var dbUrl = dbContext.Urls.FirstOrDefault(u => u.Id == id);
            if(dbUrl != null)
            {
                dbUrl.OriginalLink = url.OriginalLink;
                dbUrl.ShortLink = url.ShortLink;
                dbUrl.DateUpdated = DateTime.UtcNow;

                dbContext.Urls.Update(dbUrl);
                dbContext.SaveChanges();
            }

            return dbUrl;
        }
    }
}
