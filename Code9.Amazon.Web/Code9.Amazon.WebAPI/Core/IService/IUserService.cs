using Code9.Amazon.WebAPI.Helpers;

namespace Code9.Amazon.WebAPI.Core.IService
{
    public interface IUserService : IService<User>
    {
        PagedList<User> GetPaged(UserParams userParams);
    }
}
