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

        public async Task<User> AddAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var dbUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(dbUser != null)
            {
                dbContext.Users.Remove(dbUser);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var allUsers = await dbContext.Users.Include(u => u.Urls).ToListAsync();
            return allUsers;
        }

        public async Task<User> UpdateAsync(int id, User user)
        {
            var dbUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            
            if(dbUser != null)
            {
                dbUser.Email = user.Email;
                dbUser.FullName = user.FullName;
                
                await dbContext.SaveChangesAsync();
            }

            return dbUser;
        }
    }
}
