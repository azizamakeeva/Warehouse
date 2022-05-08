using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebApplication2.Models
{
    public partial class WarehouseContext : DbContext
    {
        public WarehouseContext()
        {
        }

        public WarehouseContext(DbContextOptions<WarehouseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Budget> Budgets { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<FinishedProduct> FinishedProducts { get; set; }
        public virtual DbSet<Ingridient> Ingridients { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<ProductSale> ProductSales { get; set; }
        public virtual DbSet<Production> Productions { get; set; }
        public virtual DbSet<RawMaterial> RawMaterials { get; set; }
        public virtual DbSet<RawMaterialPurchase> RawMaterialPurchases { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=HOME-PC;Database=Warehouse;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.ToTable("budget");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BudgetAmount)
                    .HasMaxLength(10)
                    .HasColumnName("budget_amount")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("address")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .IsFixedLength(true);

                entity.Property(e => e.Phone).HasColumnName("phone");

                entity.Property(e => e.Positions).HasColumnName("positions");

                entity.Property(e => e.Salary).HasColumnName("salary");

                entity.HasOne(d => d.PositionsNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Positions)
                    .HasConstraintName("FK_Employees_positions");
            });

            modelBuilder.Entity<FinishedProduct>(entity =>
            {
                entity.ToTable("finished_product");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .IsFixedLength(true);

                entity.Property(e => e.Sum)
                    .HasColumnType("money")
                    .HasColumnName("sum");

                entity.Property(e => e.Unit).HasColumnName("unit");

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.FinishedProducts)
                    .HasForeignKey(d => d.Unit)
                    .HasConstraintName("FK_finished_product_unit");
            });

            modelBuilder.Entity<Ingridient>(entity =>
            {
                entity.ToTable("ingridients");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.Products).HasColumnName("products");

                entity.Property(e => e.RawMaterials).HasColumnName("raw_materials");

                entity.HasOne(d => d.ProductsNavigation)
                    .WithMany(p => p.Ingridients)
                    .HasForeignKey(d => d.Products)
                    .HasConstraintName("FK_ingridients_finished_product");

                entity.HasOne(d => d.RawMaterialsNavigation)
                    .WithMany(p => p.Ingridients)
                    .HasForeignKey(d => d.RawMaterials)
                    .HasConstraintName("FK_ingridients_raw_materials");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("positions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Position1)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("position")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<ProductSale>(entity =>
            {
                entity.ToTable("product_sales");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Employee).HasColumnName("employee");

                entity.Property(e => e.Product).HasColumnName("product");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.ProductSales)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK_product_sales_Employees");

                entity.HasOne(d => d.ProductNavigation)
                    .WithMany(p => p.ProductSales)
                    .HasForeignKey(d => d.Product)
                    .HasConstraintName("FK_product_sales_finished_product");
            });

            modelBuilder.Entity<Production>(entity =>
            {
                entity.ToTable("production");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Employee).HasColumnName("employee");

                entity.Property(e => e.Product).HasColumnName("product");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.Productions)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK_production_Employees");

                entity.HasOne(d => d.ProductNavigation)
                    .WithMany(p => p.Productions)
                    .HasForeignKey(d => d.Product)
                    .HasConstraintName("FK_production_finished_product");
            });

            modelBuilder.Entity<RawMaterial>(entity =>
            {
                entity.ToTable("raw_materials");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .IsFixedLength(true);

                entity.Property(e => e.Sum).HasColumnName("sum");

                entity.Property(e => e.Unit).HasColumnName("unit");

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.RawMaterials)
                    .HasForeignKey(d => d.Unit)
                    .HasConstraintName("FK_raw_materials_unit");
            });

            modelBuilder.Entity<RawMaterialPurchase>(entity =>
            {
                entity.ToTable("raw_material_purchase");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Employee).HasColumnName("employee");

                entity.Property(e => e.RawMaterial).HasColumnName("raw_material");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.RawMaterialPurchases)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK_raw_material_purchase_Employees");

                entity.HasOne(d => d.RawMaterialNavigation)
                    .WithMany(p => p.RawMaterialPurchases)
                    .HasForeignKey(d => d.RawMaterial)
                    .HasConstraintName("FK_raw_material_purchase_raw_materials");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("unit");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
