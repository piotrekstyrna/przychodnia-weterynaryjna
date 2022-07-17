using Microsoft.EntityFrameworkCore;
using Weterynarz.Entities;

namespace Weterynarz.Classes
{
    public class VetContext : DbContext
    {
        public virtual DbSet<Animal> Animals { get; set; }
        public virtual DbSet<Disorder> Disorders { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Visit> Visits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Vet;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>(entityTypeBuilder =>
            {
                entityTypeBuilder.ToTable("Animals");
                entityTypeBuilder.HasKey(a => a.Id);
                entityTypeBuilder.Property(a => a.Name);
                entityTypeBuilder.Property(a => a.Gender);
                entityTypeBuilder.Property(a => a.BirthDate);
                entityTypeBuilder.Property(a => a.Specie);
                entityTypeBuilder.Property(a => a.Race);
            });

            modelBuilder.Entity<Disorder>(entityTypeBuilder =>
            {
                entityTypeBuilder.ToTable("Disorders");
                entityTypeBuilder.HasKey(d => d.Id);
                entityTypeBuilder.Property(d => d.Name);
                entityTypeBuilder.Property(d => d.IsHealable);
                entityTypeBuilder.Property(d => d.Medicine);
            });

            modelBuilder.Entity<Owner>(entityTypeBuilder =>
            {
                entityTypeBuilder.ToTable("Owners");
                entityTypeBuilder.HasKey(o => o.Id);
                entityTypeBuilder.Property(o => o.Name);
                entityTypeBuilder.Property(o => o.Surname);
                entityTypeBuilder.Property(o => o.PhoneNumber);
                entityTypeBuilder.Property(o => o.AnimalId);
                entityTypeBuilder.HasOne(o => o.Animal).WithMany().HasForeignKey(o => o.AnimalId);

                entityTypeBuilder.Navigation(o => o.Animal).AutoInclude();
            });

            modelBuilder.Entity<Visit>(entityTypeBuilder =>
            {
                entityTypeBuilder.ToTable("Visits");
                entityTypeBuilder.HasKey(v => v.Id);
                entityTypeBuilder.Property(v => v.Date);
                entityTypeBuilder.Property(v => v.Time);
                entityTypeBuilder.Property(v => v.Price);
                entityTypeBuilder.Property(v => v.OwnerId);
                entityTypeBuilder.Property(v => v.DisorderId);
                entityTypeBuilder.HasOne(v => v.Owner).WithMany().HasForeignKey(v => v.OwnerId);
                entityTypeBuilder.HasOne(v => v.Disorder).WithMany().HasForeignKey(v => v.DisorderId);

                entityTypeBuilder.Navigation(v => v.Owner).AutoInclude();
                entityTypeBuilder.Navigation(v => v.Disorder).AutoInclude();
            });
        }
    }
}