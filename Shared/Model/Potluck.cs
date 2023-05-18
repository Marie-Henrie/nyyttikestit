using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model
{
    public class Potluck
    {
        [Key]
        [ForeignKey("Pot")]
        public int Potluck_Id { get; set; }
        [Required,MaxLength(100)]
        public string Name { get; set; }
        public string Guid { get; set; }
        public string? HashedPassword { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Date { get; set; }
        public string? Location { get; set; }
        public virtual List<Pot> Pots { get; set; } = new List<Pot>();
    }
    public class PotluckDTO
    {
        public int Potluck_Id { get; set; }
        public string Name { get; set; }

        public DateTime Date { get; set; }
        public string? Location { get; set; }

        public string? HashedPassword { get; set; }
        public string? Description { get; set; }
    }
}
