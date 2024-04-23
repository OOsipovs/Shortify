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
        IEnumerable<Url> GetUrls();
        Url Add(Url user);
        Url GetById(int id);
        Url Update(int id, Url url);
        void Delete(int id);
    }
}
