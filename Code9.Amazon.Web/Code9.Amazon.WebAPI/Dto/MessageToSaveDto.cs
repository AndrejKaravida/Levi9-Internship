using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Dto
{
    public class MessageToSaveDto
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }

        public MessageToSaveDto()
        {
            MessageSent = DateTime.Now;
        }
    }
}
