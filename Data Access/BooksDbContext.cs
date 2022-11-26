using Microsoft.EntityFrameworkCore;

public class BooksDbContext : DbContext
{
    public BooksDbContext(DbContextOptions<BooksDbContext> options)
        : base(options)
    {    
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        
        modelBuilder.Entity<UserInfo>()
            .HasIndex(u => u.EMail)
            .IsUnique();
    }
    
    public DbSet<Book>? Books { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<UserInfo>? UserInfos { get; set; }
    public DbSet<Order>? Orders { get; set; }
    public DbSet<Cart>? Carts { get; set; }
}    