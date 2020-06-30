using Code9.Amazon.WebAPI.Core.IRepository;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Helpers;
using System.Linq;

namespace Code9.Amazon.WebAPI.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IUserRepository _repos;
        public UserService(IUserRepository repo) : base(repo)
        {
            _repos = repo;
        }
        public PagedList<User> GetPaged(UserParams userParams)
        {
            var users = _repos.GetUserAsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);

            return new PagedList<User>(users, userParams.PageNumber, userParams.PageSize);
        }
    }
}
