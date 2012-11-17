using System.ComponentModel.DataAnnotations;

namespace FG12.Models
{
    public class Group
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}