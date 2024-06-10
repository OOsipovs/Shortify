using Shortify.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortify.Data.Services
{
    public interface IUrlsService
    {
        Task<IEnumerable<Url>> GetUrlsAsync(string userId, bool isAdmin);
        Task<Url> AddAsync(Url user);
        Task<Url> GetByIdAsync(int id);
        Task<Url> UpdateAsync(int id, Url url);
        Task DeleteAsync(int id);
    }
}
