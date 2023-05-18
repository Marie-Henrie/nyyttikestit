using Microsoft.EntityFrameworkCore;

namespace Server.Model
{
    public class NyyttiDbContext : DbContext
    {
        public NyyttiDbContext()
        {

        }
        public NyyttiDbContext(DbContextOptions<NyyttiDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Potluck> Potlucks { get; set; }
        public virtual DbSet<Pot> Pots { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
    }
}
