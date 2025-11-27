using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Infrastructure.Database;

public sealed class OguzlarBelediyesiDbContext : DbContext
{
    public OguzlarBelediyesiDbContext(DbContextOptions<OguzlarBelediyesiDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Tender> Tenders => Set<Tender>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).HasMaxLength(64);
            entity.HasIndex(u => u.Username).IsUnique();
        });

        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.ToTable("Announcements");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Title).IsRequired().HasMaxLength(200);
            entity.Property(a => a.Summary).IsRequired().HasMaxLength(1000);
            entity.Property(a => a.Content).IsRequired();
            entity.Property(a => a.Category).HasMaxLength(128);
            entity.Property(a => a.Date).HasMaxLength(64);
            entity.Property(a => a.Slug).IsRequired().HasMaxLength(256);
            entity.Property(a => a.PublishedAt).IsRequired();
            entity.HasIndex(a => a.Slug).IsUnique();
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("Events");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Date).HasMaxLength(64);
            entity.Property(e => e.EventDate).IsRequired();
            entity.Property(e => e.Image).HasMaxLength(512);
            entity.Property(e => e.Slug).IsRequired().HasMaxLength(256);
            entity.HasIndex(e => e.Slug).IsUnique();
        });

        modelBuilder.Entity<Tender>(entity =>
        {
            entity.ToTable("Tenders");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Description).IsRequired().HasMaxLength(2000);
            entity.Property(t => t.Date).HasMaxLength(64);
            entity.Property(t => t.PublishedAt).IsRequired();
            entity.Property(t => t.RegistrationNumber).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Status).IsRequired().HasMaxLength(64);
            entity.Property(t => t.EstimatedValue);
            entity.Property(t => t.Slug).IsRequired().HasMaxLength(256);
            entity.HasIndex(t => t.Slug).IsUnique();
        });
    }
}
