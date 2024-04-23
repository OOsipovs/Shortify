using Shortify.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortify.Data.Services
{
    public interface IUsersService
    {
        IEnumerable<User> GetUsers();
        User Add(User url);
        User GetById(int id);
        User Update(int id, User url);
        void Delete(int id);
    }
}
