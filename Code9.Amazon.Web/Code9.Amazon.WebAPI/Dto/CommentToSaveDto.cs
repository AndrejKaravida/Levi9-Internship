using System.ComponentModel.DataAnnotations;

namespace Code9.Amazon.WebAPI.Dto
{
    public class CommentToSaveDto
    {
        [Required]
        [StringLength(500)]
        public string Text { get; set; }
        [Required]
        public int VehicleId { get; set; }
    }
}
