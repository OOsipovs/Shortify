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
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> AddAsync(User url);
        Task<User> GetByIdAsync(int id);
        Task<User> UpdateAsync(int id, User url);
        Task DeleteAsync(int id);
    }
}
