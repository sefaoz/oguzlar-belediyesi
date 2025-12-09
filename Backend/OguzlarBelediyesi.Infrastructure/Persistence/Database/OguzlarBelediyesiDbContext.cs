using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Domain.Entities.Configuration;
using OguzlarBelediyesi.Domain.Entities.Messages;
using OguzlarBelediyesi.Infrastructure.Persistence.Entities;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Database;

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
    public DbSet<NewsEntity> NewsItems => Set<NewsEntity>();
    public DbSet<PageContentEntity> PageContents => Set<PageContentEntity>();
    public DbSet<GalleryFolderEntity> GalleryFolders => Set<GalleryFolderEntity>();
    public DbSet<GalleryImageEntity> GalleryImages => Set<GalleryImageEntity>();
    public DbSet<CouncilDocumentEntity> CouncilDocuments => Set<CouncilDocumentEntity>();
    public DbSet<KvkkDocumentEntity> KvkkDocuments => Set<KvkkDocumentEntity>();
    public DbSet<MunicipalUnitEntity> MunicipalUnits => Set<MunicipalUnitEntity>();
    public DbSet<VehicleEntity> Vehicles => Set<VehicleEntity>();
    public DbSet<SliderEntity> Sliders => Set<SliderEntity>();
    public DbSet<MenuItemEntity> MenuItems => Set<MenuItemEntity>();
    public DbSet<SerilogLogEntry> SerilogLogs => Set<SerilogLogEntry>();

    public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

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

        modelBuilder.Entity<SiteSetting>(entity =>
        {
            entity.ToTable("SiteSettings");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Key).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Value).IsRequired(); // LongText by default in MySQL for string if no length
            entity.Property(s => s.GroupKey).IsRequired().HasMaxLength(50);
            entity.Property(s => s.Description).HasMaxLength(500);
            entity.HasIndex(s => new { s.GroupKey, s.Key }).IsUnique();
        });

        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.ToTable("Announcements");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Title).IsRequired().HasMaxLength(200);
            entity.Property(a => a.Summary).IsRequired().HasMaxLength(1000);
            entity.Property(a => a.Content).IsRequired();
            entity.Property(a => a.Category).HasMaxLength(128);
            entity.Property(a => a.Date).IsRequired();
            entity.Property(a => a.Slug).IsRequired().HasMaxLength(256);
            entity.HasIndex(a => a.Slug).IsUnique();
        });


        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("Events");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Location).IsRequired().HasMaxLength(200);

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
            entity.Property(t => t.TenderDate).IsRequired();
            entity.Property(t => t.RegistrationNumber).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Status).IsRequired().HasMaxLength(64);
            entity.Property(t => t.EstimatedValue);
            entity.Property(t => t.DocumentsJson).IsRequired();
            entity.Property(t => t.Slug).IsRequired().HasMaxLength(256);
            entity.HasIndex(t => t.Slug).IsUnique();
        });

        modelBuilder.Entity<NewsEntity>(entity =>
        {
            entity.ToTable("News");
            entity.HasKey(n => n.Id);
            entity.Property(n => n.Title).IsRequired().HasMaxLength(200);
            entity.Property(n => n.Slug).IsRequired().HasMaxLength(256);
            entity.Property(n => n.Description).IsRequired().HasMaxLength(2000);
            entity.Property(n => n.Image).IsRequired().HasMaxLength(512);
            entity.Property(n => n.Date).IsRequired();
            entity.Property(n => n.PhotosJson).IsRequired();
            entity.HasIndex(n => n.Slug).IsUnique();
        });

        modelBuilder.Entity<PageContentEntity>(entity =>
        {
            entity.ToTable("PageContents");
            entity.HasKey(pc => pc.Id);
            entity.Property(pc => pc.Key).IsRequired().HasMaxLength(150);
            entity.Property(pc => pc.Title).IsRequired().HasMaxLength(200);
            entity.Property(pc => pc.Subtitle).HasMaxLength(200);
            entity.Property(pc => pc.ParagraphsJson).IsRequired();
            entity.Property(pc => pc.ImageUrl).HasMaxLength(512);
            entity.Property(pc => pc.MapEmbedUrl).HasMaxLength(512);
            entity.Property(pc => pc.ContactDetailsJson);
            entity.HasIndex(pc => pc.Key).IsUnique();
        });

        modelBuilder.Entity<GalleryFolderEntity>(entity =>
        {
            entity.ToTable("GalleryFolders");
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Title).IsRequired().HasMaxLength(200);
            entity.Property(g => g.Slug).IsRequired().HasMaxLength(256);
            entity.Property(g => g.CoverImage).HasMaxLength(512);
            entity.Property(g => g.ImageCount).IsRequired();
            entity.Property(g => g.Date).HasMaxLength(64);
            entity.HasIndex(g => g.Slug).IsUnique();
        });

        modelBuilder.Entity<GalleryImageEntity>(entity =>
        {
            entity.ToTable("GalleryImages");
            entity.HasKey(g => g.Id);
            entity.Property(g => g.FolderId).IsRequired();
            entity.Property(g => g.Url).IsRequired().HasMaxLength(512);
            entity.Property(g => g.ThumbnailUrl).IsRequired().HasMaxLength(512);
            entity.Property(g => g.Title).HasMaxLength(256);
            entity.HasIndex(g => g.FolderId);
        });

        modelBuilder.Entity<CouncilDocumentEntity>(entity =>
        {
            entity.ToTable("CouncilDocuments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Type).HasMaxLength(128);
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1024);
            entity.Property(e => e.FileUrl).HasMaxLength(512);
        });

        modelBuilder.Entity<KvkkDocumentEntity>(entity =>
        {
            entity.ToTable("KvkkDocuments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Type).HasMaxLength(128);
            entity.Property(e => e.FileUrl).IsRequired().HasMaxLength(512);
        });

        modelBuilder.Entity<MunicipalUnitEntity>(entity =>
        {
            entity.ToTable("MunicipalUnits");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Slug).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Content).HasMaxLength(2000);
            entity.Property(e => e.Icon).HasMaxLength(64);
            entity.Property(e => e.StaffJson).IsRequired();
            entity.HasIndex(e => e.Slug).IsUnique();
        });

        modelBuilder.Entity<VehicleEntity>(entity =>
        {
            entity.ToTable("Vehicles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(128);
            entity.Property(e => e.Plate).IsRequired().HasMaxLength(32);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1024);
            entity.Property(e => e.ImageUrl).HasMaxLength(512);
        });

        modelBuilder.Entity<SliderEntity>(entity =>
        {
            entity.ToTable("Sliders");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(512);
            entity.Property(e => e.Link).HasMaxLength(512);
            entity.Property(e => e.Order);
            entity.Property(e => e.IsActive);
        });

        modelBuilder.Entity<MenuItemEntity>(entity =>
        {
            entity.ToTable("MenuItems");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Url).HasMaxLength(512);
            entity.Property(e => e.ParentId);
            entity.Property(e => e.Order);
            entity.Property(e => e.IsVisible);
            entity.Property(e => e.Target).HasMaxLength(16);
            entity.HasIndex(e => e.ParentId);
        });

        modelBuilder.Entity<SerilogLogEntry>(entity =>
        {
            entity.ToTable("SerilogLogs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int");
            entity.Property(e => e.Timestamp).IsRequired();
            entity.Property(e => e.Level).IsRequired().HasMaxLength(64);
            entity.Property(e => e.MessageTemplate).IsRequired().HasMaxLength(1024);
            entity.Property(e => e.RenderedMessage).IsRequired().HasColumnType("longtext");
            entity.Property(e => e.Exception).HasColumnType("longtext");
            entity.Property(e => e.Properties).IsRequired().HasColumnType("longtext");
            entity.Property(e => e.Endpoint).HasMaxLength(512);
            entity.Property(e => e.DurationMs);
            entity.Property(e => e.Username).HasMaxLength(256);
            entity.HasIndex(e => e.Timestamp);
            entity.HasIndex(e => e.Level);
        });

        modelBuilder.Entity<ContactMessage>(entity =>
        {
            entity.ToTable("ContactMessages");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Message).IsRequired().HasColumnType("longtext");
            entity.Property(e => e.MessageType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.HasIndex(e => e.MessageType);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.IsRead);
        });
    }
}
