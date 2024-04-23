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

        public User Add(User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return user;
        }

        public void Delete(int id)
        {
            var dbUser = dbContext.Users.FirstOrDefault(u => u.Id == id);
            if(dbUser != null)
            {
                dbContext.Users.Remove(dbUser);
                dbContext.SaveChanges();
            }
        }

        public User GetById(int id)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            var allUsers = dbContext.Users.Include(u => u.Urls).ToList();
            return allUsers;
        }

        public User Update(int id, User user)
        {
            var dbUser = dbContext.Users.FirstOrDefault(u => u.Id == id);
            
            if(dbUser != null)
            {
                dbUser.Email = user.Email;
                dbUser.FullName = user.FullName;
                dbContext.Update(dbUser);
                dbContext.SaveChanges();
            }

            return dbUser;
        }
    }
}
