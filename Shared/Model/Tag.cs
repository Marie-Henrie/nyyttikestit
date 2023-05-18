using System.ComponentModel.DataAnnotations;

namespace Server.Model
{
    public class Tag
    {
        [Key]
        public int Tag_Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual List<Pot> Pots { get; set; } = new List<Pot>();
    }
    public class TagDTO
    {
        public int Tag_Id { get; set; }
        public string Name { get; set; }
    }
}
