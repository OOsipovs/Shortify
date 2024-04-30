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
        Task<IEnumerable<AppUser>> GetUsersAsync();
       
    }
}
