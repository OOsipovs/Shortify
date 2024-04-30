using Microsoft.EntityFrameworkCore;
using Shortify.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortify.Data.Services
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext dbContext;

        public UsersService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            var allUsers = await dbContext.Users.Include(u => u.Urls).ToListAsync();
            return allUsers;
        }

    }
}
