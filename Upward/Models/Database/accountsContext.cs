using Microsoft.EntityFrameworkCore;

namespace Upward.Models.Database
{
    public partial class accountsContext : DbContext
    {
        public virtual DbSet<Userprofile> Userprofile { get; set; }
        public virtual DbSet<Project> Project { get; set; }

        public accountsContext(DbContextOptions<accountsContext> options) : base(options) {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("project");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(39)
                    .HasColumnName("name");

                entity.Property(e => e.Userid)
                .HasColumnName("userid");

                entity.Property(e => e.Private)
                    .IsRequired()
                    .HasColumnName("private");
            });

            modelBuilder.Entity<Userprofile>(entity =>
            {
                entity.ToTable("userprofile");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Size)
                    .HasColumnName("size")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(320)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("password");
            });
        }
    }
}
