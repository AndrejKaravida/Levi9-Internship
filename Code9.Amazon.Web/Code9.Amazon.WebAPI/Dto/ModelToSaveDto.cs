using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Dto
{
    public class ModelToSaveDto
    {
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        public int MakeId { get; set; }
    }
}
