using Microsoft.EntityFrameworkCore;
using Shortify.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shortify.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Url> Urls { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
