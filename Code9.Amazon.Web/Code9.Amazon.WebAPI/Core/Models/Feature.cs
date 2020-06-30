using System.ComponentModel.DataAnnotations;

namespace Code9.Amazon.WebAPI.Core.Models
{
    public class Feature
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
    }
}
