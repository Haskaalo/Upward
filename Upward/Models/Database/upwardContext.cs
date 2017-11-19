using Microsoft.EntityFrameworkCore;

namespace Upward.Models.Database
{
    public partial class upwardContext : DbContext
    {
        public virtual DbSet<Pkgapikey> Pkgapikey { get; set; }
        public virtual DbSet<Pkgfile> Pkgfile { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Userprofile> Userprofile { get; set; }

        public upwardContext(DbContextOptions<upwardContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pkgapikey>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.ToTable("pkgapikey");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("key")
                    .HasColumnType("uuid");

                entity.Property(e => e.Created)
                    .IsRequired()
                    .HasColumnName("created")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Project)
                    .IsRequired()
                    .HasColumnName("project");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Pkgfile>(entity =>
            {
                entity.ToTable("pkgfile");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasColumnName("filename")
                    .HasColumnType("varchar(255)[]");

                entity.Property(e => e.Branch)
                    .IsRequired()
                    .HasMaxLength(39)
                    .HasColumnName("branch");

                entity.Property(e => e.Size)
                    .HasColumnName("size");

                entity.Property(e => e.Project).HasColumnName("project");

                entity.Property(e => e.Major)
                    .IsRequired()
                    .HasColumnName("major");

                entity.Property(e => e.Minor)
                    .IsRequired()
                    .HasColumnName("minor");

                entity.Property(e => e.Patch)
                    .IsRequired()
                    .HasColumnName("patch");
            });

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
