using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace NTTUserApp.Data.Entities;
public class MyDbContext : DbContext
{
    public MyDbContext() :base() { }
    public MyDbContext(DbContextOptions<MyDbContext> options) :base(options){}
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=ISTN32156\\SQLEXPRESS;Database=UserManagement;user id=dbuser_UserManagement;Password=asd123;Trusted_Connection=True;TrustServerCertificate=True");
        }        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.UseCollation("Turkish_CI_AS");

        modelBuilder.Entity<User>(entity => {
           

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x=> x.Name).IsRequired().HasMaxLength(100);
            entity.HasMany(x => x.PhoneNumbers).WithOne(x => x.User).HasForeignKey(x => x.Id);
        });

        modelBuilder.Entity<PhoneNumber>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.PhoneNo).IsRequired().HasMaxLength(11);
            entity.HasOne(x => x.User).WithMany(x => x.PhoneNumbers).HasForeignKey(x => x.UserId);
        });

        base.OnModelCreating(modelBuilder);

    }
}