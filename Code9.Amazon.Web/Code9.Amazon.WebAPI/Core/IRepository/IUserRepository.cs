using Code9.Amazon.WebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Core.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        IQueryable<User> GetUserAsQueryable();

    }
}
