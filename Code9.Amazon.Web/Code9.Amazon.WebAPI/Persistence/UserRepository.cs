using Code9.Amazon.WebAPI.Core.IRepository;
using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Helpers;
using Code9.Amazon.WebAPI.Persistance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Persistence
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public IQueryable<User> GetUserAsQueryable()
        {
            return _context.Users.AsQueryable();
        }
    }
}
