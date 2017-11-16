namespace GameIn
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GameInDb : DbContext
    {
        public GameInDb()
            : base("name=GameInDb")
        {

        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.LastIP)
                .IsUnicode(false);
        }
    }
}
