using System.Collections.Generic;

namespace Code9.Amazon.WebAPI.Dto
{
    public class MakeDto : KeyValuePairDto
    {
        public ICollection<KeyValuePairDto> Models { get; set; }
    }
}
