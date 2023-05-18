using System.ComponentModel.DataAnnotations;

namespace Server.Model
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
