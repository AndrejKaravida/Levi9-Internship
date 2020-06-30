using System.ComponentModel.DataAnnotations;

namespace Code9.Amazon.WebAPI.Core.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Text { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
