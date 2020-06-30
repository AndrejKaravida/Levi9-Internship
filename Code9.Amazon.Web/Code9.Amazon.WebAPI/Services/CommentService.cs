using Code9.Amazon.WebAPI.Core.IRepository;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Services
{
    public class CommentService : Service<Comment>, ICommentService
    {
        private readonly ICommentRepository _repo;

        public CommentService(ICommentRepository repo) : base(repo)
        {
            _repo = repo;
        }


        public async Task<IEnumerable<Comment>> GetAllForVehicleAsync(int vehicleId)
        {
            return await _repo.GetAllForVehicle(vehicleId);
        }

    }
}
