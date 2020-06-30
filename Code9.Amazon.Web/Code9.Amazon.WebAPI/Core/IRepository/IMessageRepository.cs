using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Core.IRepository
{
    public interface IMessageRepository : IRepository<Message>
    {
        IQueryable<Message> GetMessagesQueryable();
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}
