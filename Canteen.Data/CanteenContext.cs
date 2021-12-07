using Microsoft.EntityFrameworkCore;

namespace Canteen.DataAccess;

public partial class CanteenContext : DbContext
{
    public CanteenContext()
    {
    }

    public CanteenContext(DbContextOptions<CanteenContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Employee> Employees { get; set; } = null!;
    public virtual DbSet<EmployeeCake> EmployeeCakes { get; set; } = null!;
    public virtual DbSet<EmployeeLunch> EmployeeLunches { get; set; } = null!;
    public virtual DbSet<Item> Items { get; set; } = null!;
    public virtual DbSet<LunchCancellation> LunchCancellations { get; set; } = null!;
    public virtual DbSet<LunchMenu> LunchMenus { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
    public virtual DbSet<Week> Weeks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.HasIndex(e => e.Name, "AK_Category")
                .IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

            entity.Property(e => e.FirstName).HasMaxLength(50);

            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.Property(e => e.Password).HasMaxLength(50);
        });

        modelBuilder.Entity<EmployeeCake>(entity =>
        {
            entity.HasKey(e => new {e.EmployeeId, e.ItemId});

            entity.ToTable("EmployeeCake");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");

            entity.HasOne(d => d.Employee)
                .WithMany(p => p.EmployeeCakes)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeCake_Employee");

            entity.HasOne(d => d.Item)
                .WithMany(p => p.EmployeeCakes)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeCake_Item");

            entity.HasOne(d => d.Week)
                .WithMany(p => p.EmployeeCakes)
                .HasForeignKey(d => new {d.Number, d.Year})
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeCake_Week");
        });

        modelBuilder.Entity<EmployeeLunch>(entity =>
        {
            entity.HasKey(e => new {e.LunchMenuId, e.EmployeeId});

            entity.ToTable("EmployeeLunch");

            entity.Property(e => e.LunchMenuId).HasColumnName("LunchMenuID");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

            entity.HasOne(d => d.Employee)
                .WithMany(p => p.EmployeeLunches)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeLunch_Employee");

            entity.HasOne(d => d.LunchMenu)
                .WithMany(p => p.EmployeeLunches)
                .HasForeignKey(d => d.LunchMenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeLunch_LunchMenu");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item");

            entity.HasIndex(e => e.CategoryId, "IX_Item");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Items)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Item_Category");

            entity.HasMany(d => d.Employees)
                .WithMany(p => p.Items)
                .UsingEntity<Dictionary<string, object>>(
                    "FavouriteItem",
                    l => l.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FavouriteItem_Employee"),
                    r => r.HasOne<Item>().WithMany().HasForeignKey("ItemId").OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_FavouriteItem_Item"),
                    j =>
                    {
                        j.HasKey("ItemId", "EmployeeId");

                        j.ToTable("FavouriteItem");

                        j.IndexerProperty<int>("ItemId").HasColumnName("ItemID");

                        j.IndexerProperty<int>("EmployeeId").HasColumnName("EmployeeID");
                    });
        });

        modelBuilder.Entity<LunchCancellation>(entity =>
        {
            entity.ToTable("LunchCancellation");

            entity.HasIndex(e => e.LunchMenuId, "IX_LunchCancellation");

            entity.Property(e => e.LunchCancellationId)
                .ValueGeneratedNever()
                .HasColumnName("LunchCancellationID");

            entity.Property(e => e.LunchMenuId).HasColumnName("LunchMenuID");

            entity.Property(e => e.Message).HasMaxLength(250);

            entity.HasOne(d => d.LunchMenu)
                .WithMany(p => p.LunchCancellations)
                .HasForeignKey(d => d.LunchMenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LunchCancellation_LunchMenu");
        });

        modelBuilder.Entity<LunchMenu>(entity =>
        {
            entity.ToTable("LunchMenu");

            entity.Property(e => e.LunchMenuId).HasColumnName("LunchMenuID");

            entity.Property(e => e.FridayItemId).HasColumnName("FridayItemID");

            entity.Property(e => e.MondayItemId).HasColumnName("MondayItemID");

            entity.Property(e => e.ThursdayItemId).HasColumnName("ThursdayItemID");

            entity.Property(e => e.TuesdayItemId).HasColumnName("TuesdayItemID");

            entity.Property(e => e.WednesdayItemId).HasColumnName("WednesdayItemID");

            entity.HasOne(d => d.FridayItem)
                .WithMany(p => p.LunchMenuFridayItems)
                .HasForeignKey(d => d.FridayItemId)
                .HasConstraintName("FK_LunchMenu_Item_Friday");

            entity.HasOne(d => d.MondayItem)
                .WithMany(p => p.LunchMenuMondayItems)
                .HasForeignKey(d => d.MondayItemId)
                .HasConstraintName("FK_LunchMenu_Item_Monday");

            entity.HasOne(d => d.ThursdayItem)
                .WithMany(p => p.LunchMenuThursdayItems)
                .HasForeignKey(d => d.ThursdayItemId)
                .HasConstraintName("FK_LunchMenu_Item_Thursday");

            entity.HasOne(d => d.TuesdayItem)
                .WithMany(p => p.LunchMenuTuesdayItems)
                .HasForeignKey(d => d.TuesdayItemId)
                .HasConstraintName("FK_LunchMenu_Item_Tuesday");

            entity.HasOne(d => d.WednesdayItem)
                .WithMany(p => p.LunchMenuWednesdayItems)
                .HasForeignKey(d => d.WednesdayItemId)
                .HasConstraintName("FK_LunchMenu_Item_Wednesday");

            entity.HasOne(d => d.Week)
                .WithMany(p => p.LunchMenus)
                .HasForeignKey(d => new {d.Number, d.Year})
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LunchMenu_Week");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

            entity.HasOne(d => d.Employee)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Employee");

            entity.HasOne(d => d.Week)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => new {d.Number, d.Year})
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Week");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => new {e.OrderId, e.ItemId});

            entity.ToTable("OrderItem");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");

            entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Item)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Item");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Order");
        });

        modelBuilder.Entity<Week>(entity =>
        {
            entity.HasKey(e => new {e.Number, e.Year});

            entity.ToTable("Week");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}