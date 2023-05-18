using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model
{
    public class Pot
    {
        [Key]
        public int Pot_Id { get; set; }
        [Required, MaxLength(20)]
        public string Creator { get; set; }
        //public int Potluck_Id { get; set; }
        public int Course_Id { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required, MaxLength(100)]
        public string DishName { get;set; }
        public DateTime Created { get; set; }

        public virtual List<Tag> Tags { get; set; }

        public Potluck Potluck { get; set; }
    }
    public class PotDTO
    {
        public int Pot_Id { get; set; }
        public string Creator { get; set; }
        public int Course_Id { get; set; }
        public string Description { get; set; }
        public string DishName { get; set; }

        public virtual List<int> tag_ids { get; set; }
    }
}
