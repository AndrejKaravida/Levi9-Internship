using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Core.Models
{
    public class Image
    {
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }
        public int VehicleId { get; set; }
        public bool IsMain { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
