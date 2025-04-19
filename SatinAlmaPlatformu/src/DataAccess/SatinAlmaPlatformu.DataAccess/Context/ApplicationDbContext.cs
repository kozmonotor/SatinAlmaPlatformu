using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SatinAlmaPlatformu.Entities.Models;
using System.IO;
using System.Linq.Expressions;

namespace SatinAlmaPlatformu.DataAccess.Context;

/// <summary>
/// Entity Framework Core veritabanı bağlantı ve işlemlerini yöneten DbContext sınıfı
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Database entity setleri
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
    public DbSet<PurchaseRequestItem> PurchaseRequestItems { get; set; }
    public DbSet<ApprovalFlow> ApprovalFlows { get; set; }
    public DbSet<ApprovalFlowStep> ApprovalFlowSteps { get; set; }
    public DbSet<ApprovalStep> ApprovalSteps { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<SupplierCategory> SupplierCategories { get; set; }
    public DbSet<BidRequest> BidRequests { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<BidItem> BidItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<SupplierRating> SupplierRatings { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Dışarıdan DbContext options verilmezse, appsettings.json'dan bağlantı bilgisini al
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Soft Delete filtresi
        ApplySoftDeleteFilter(modelBuilder);
        
        // Entity ilişkilerini ve özelliklerini yapılandır
        ConfigureEntityRelationships(modelBuilder);
        
        // Index tanımlamaları
        ConfigureIndexes(modelBuilder);
    }

    private void ApplySoftDeleteFilter(ModelBuilder modelBuilder)
    {
        // Tüm sorgulamalarda IsDeleted = false olan kayıtları getir
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Entity BaseEntity'den türeyen (IsDeleted özelliği olan) sınıflara soft delete filtresi uygula
            if (typeof(Core.Entities.BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.PropertyOrField(parameter, "IsDeleted");
                var falseValue = Expression.Constant(false);
                var condition = Expression.Equal(property, falseValue);
                var lambda = Expression.Lambda(condition, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    private void ConfigureEntityRelationships(ModelBuilder modelBuilder)
    {
        // UserRole ilişkileri
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // RolePermission ilişkileri
        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Department - User ilişkisi
        modelBuilder.Entity<User>()
            .HasOne(u => u.Department)
            .WithMany(d => d.Users)
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull);

        // Category self-referencing ilişkisi (parent-child)
        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.ChildCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // PurchaseRequest ilişkileri
        modelBuilder.Entity<PurchaseRequest>()
            .HasOne(pr => pr.RequestedBy)
            .WithMany(u => u.Requests)
            .HasForeignKey(pr => pr.RequestedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseRequest>()
            .HasOne(pr => pr.Department)
            .WithMany(d => d.Requests)
            .HasForeignKey(pr => pr.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull);

        // PurchaseRequestItem ilişkisi
        modelBuilder.Entity<PurchaseRequestItem>()
            .HasOne(pri => pri.PurchaseRequest)
            .WithMany(pr => pr.Items)
            .HasForeignKey(pri => pri.PurchaseRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        // ApprovalFlowStep ilişkisi
        modelBuilder.Entity<ApprovalFlowStep>()
            .HasOne(afs => afs.ApprovalFlow)
            .WithMany(af => af.Steps)
            .HasForeignKey(afs => afs.ApprovalFlowId)
            .OnDelete(DeleteBehavior.Cascade);

        // ApprovalStep ilişkileri
        modelBuilder.Entity<ApprovalStep>()
            .HasOne(ast => ast.PurchaseRequest)
            .WithMany(pr => pr.ApprovalSteps)
            .HasForeignKey(ast => ast.PurchaseRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        // SupplierCategory ilişkileri
        modelBuilder.Entity<SupplierCategory>()
            .HasOne(sc => sc.Supplier)
            .WithMany(s => s.SupplierCategories)
            .HasForeignKey(sc => sc.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SupplierCategory>()
            .HasOne(sc => sc.Category)
            .WithMany()
            .HasForeignKey(sc => sc.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // BidRequest ilişkileri
        modelBuilder.Entity<BidRequest>()
            .HasOne(br => br.PurchaseRequest)
            .WithMany(pr => pr.BidRequests)
            .HasForeignKey(br => br.PurchaseRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BidRequest>()
            .HasOne(br => br.Supplier)
            .WithMany(s => s.BidRequests)
            .HasForeignKey(br => br.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        // Bid ilişkileri
        modelBuilder.Entity<Bid>()
            .HasOne(b => b.BidRequest)
            .WithOne(br => br.Bid)
            .HasForeignKey<Bid>(b => b.BidRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Bid>()
            .HasOne(b => b.Supplier)
            .WithMany(s => s.Bids)
            .HasForeignKey(b => b.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        // BidItem ilişkileri
        modelBuilder.Entity<BidItem>()
            .HasOne(bi => bi.Bid)
            .WithMany(b => b.Items)
            .HasForeignKey(bi => bi.BidId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BidItem>()
            .HasOne(bi => bi.PurchaseRequestItem)
            .WithMany(pri => pri.BidItems)
            .HasForeignKey(bi => bi.PurchaseRequestItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Order ilişkileri
        modelBuilder.Entity<Order>()
            .HasOne(o => o.PurchaseRequest)
            .WithMany(pr => pr.Orders)
            .HasForeignKey(o => o.PurchaseRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Supplier)
            .WithMany(s => s.Orders)
            .HasForeignKey(o => o.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        // OrderItem ilişkileri
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.PurchaseRequestItem)
            .WithMany(pri => pri.OrderItems)
            .HasForeignKey(oi => oi.PurchaseRequestItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.BidItem)
            .WithMany()
            .HasForeignKey(oi => oi.BidItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Attachment ilişkileri - burada bütün ilişkileri yazmak yerine ReferenceType ve ReferenceId ile yönetiyoruz
        // ancak doğrudan gezinme için bazı ilişkileri belirtiyoruz
        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.PurchaseRequest)
            .WithMany(pr => pr.Attachments)
            .HasForeignKey(a => a.PurchaseRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.BidRequest)
            .WithMany(br => br.Attachments)
            .HasForeignKey(a => a.BidRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.Bid)
            .WithMany(b => b.Attachments)
            .HasForeignKey(a => a.BidId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.Order)
            .WithMany(o => o.Attachments)
            .HasForeignKey(a => a.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // SupplierRating ilişkileri
        modelBuilder.Entity<SupplierRating>()
            .HasOne(sr => sr.Supplier)
            .WithMany(s => s.Ratings)
            .HasForeignKey(sr => sr.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SupplierRating>()
            .HasOne(sr => sr.Order)
            .WithMany()
            .HasForeignKey(sr => sr.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Notification ilişkisi
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureIndexes(ModelBuilder modelBuilder)
    {
        // User tablosu için indexler
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Department tablosu için indexler
        modelBuilder.Entity<Department>()
            .HasIndex(d => d.Name)
            .IsUnique();

        modelBuilder.Entity<Department>()
            .HasIndex(d => d.Code)
            .IsUnique()
            .HasFilter("[Code] IS NOT NULL");

        // Role tablosu için indexler
        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();

        // Permission tablosu için indexler
        modelBuilder.Entity<Permission>()
            .HasIndex(p => p.Code)
            .IsUnique();

        // Supplier tablosu için indexler
        modelBuilder.Entity<Supplier>()
            .HasIndex(s => s.Name);

        modelBuilder.Entity<Supplier>()
            .HasIndex(s => s.Email)
            .HasFilter("[Email] IS NOT NULL");

        // Order tablosu için indexler
        modelBuilder.Entity<Order>()
            .HasIndex(o => o.OrderNumber)
            .IsUnique();

        // PurchaseRequest tablosu için indexler
        modelBuilder.Entity<PurchaseRequest>()
            .HasIndex(pr => new { pr.RequestedById, pr.Status });

        // Notification tablosu için indexler
        modelBuilder.Entity<Notification>()
            .HasIndex(n => new { n.UserId, n.IsRead });
    }

    // Entity değişikliklerini kaydederken BaseEntity alanlarının doldurulması
    public override int SaveChanges()
    {
        UpdateBaseEntityProperties();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateBaseEntityProperties();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateBaseEntityProperties()
    {
        var entries = ChangeTracker.Entries().Where(e => 
            e.Entity is Core.Entities.BaseEntity && 
            (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (Core.Entities.BaseEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
                // CreatedBy alanı normalde burada kullanıcı bilgisinden doldurulacak
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.ModifiedAt = DateTime.UtcNow;
                // ModifiedBy alanı normalde burada kullanıcı bilgisinden doldurulacak
            }
        }
    }
} 