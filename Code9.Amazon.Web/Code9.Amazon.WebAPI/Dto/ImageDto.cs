using System.ComponentModel.DataAnnotations;

namespace Code9.Amazon.WebAPI.Dto
{
    public class ImageDto
    {
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }

        public bool IsMain { get; set; }
    }
}
