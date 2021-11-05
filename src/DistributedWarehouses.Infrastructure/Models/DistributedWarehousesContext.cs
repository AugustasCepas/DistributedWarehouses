using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DistributedWarehouses.Infrastructure.Models
{
    public partial class DistributedWarehousesContext : DbContext
    {
        public DistributedWarehousesContext()
        {
        }

        public DistributedWarehousesContext(DbContextOptions<DistributedWarehousesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<ReservationItem> ReservationItems { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }
        public virtual DbSet<WarehouseItem> WarehouseItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:distributed-warehouses.database.windows.net,1433;Database=DistributedWarehouses;User ID=cepas;Password=Augustas!;Trusted_Connection=False;Encrypt=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoice");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.HasKey(e => new { e.Item, e.Warehouse, e.Invoice })
                    .HasName("InvoiceItem_PK");

                entity.ToTable("InvoiceItem");

                entity.Property(e => e.Item)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.InvoiceNavigation)
                    .WithMany(p => p.InvoiceItems)
                    .HasForeignKey(d => d.Invoice)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("InvoiceItem_FK_2");

                entity.HasOne(d => d.ItemNavigation)
                    .WithMany(p => p.InvoiceItems)
                    .HasForeignKey(d => d.Item)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("InvoiceItem_FK");

                entity.HasOne(d => d.WarehouseNavigation)
                    .WithMany(p => p.InvoiceItems)
                    .HasForeignKey(d => d.Warehouse)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("InvoiceItem_FK_1");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Sku)
                    .HasName("Item_PK");

                entity.ToTable("Item");

                entity.Property(e => e.Sku)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SKU");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("Reservation");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.ExpirationTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<ReservationItem>(entity =>
            {
                entity.HasKey(e => new { e.Item, e.Warehouse, e.Reservation })
                    .HasName("ReservationItem_PK");

                entity.ToTable("ReservationItem");

                entity.Property(e => e.Item)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.ItemNavigation)
                    .WithMany(p => p.ReservationItems)
                    .HasForeignKey(d => d.Item)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ReservationItem_FK");

                entity.HasOne(d => d.ReservationNavigation)
                    .WithMany(p => p.ReservationItems)
                    .HasForeignKey(d => d.Reservation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ReservationItem_FK_2");

                entity.HasOne(d => d.WarehouseNavigation)
                    .WithMany(p => p.ReservationItems)
                    .HasForeignKey(d => d.Warehouse)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ReservationItem_FK_1");
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.ToTable("Warehouse");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WarehouseItem>(entity =>
            {
                entity.HasKey(e => new { e.Item, e.Warehouse })
                    .HasName("WarehouseItem_PK");

                entity.ToTable("WarehouseItem");

                entity.Property(e => e.Item)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.ItemNavigation)
                    .WithMany(p => p.WarehouseItems)
                    .HasForeignKey(d => d.Item)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WarehouseItem_FK");

                entity.HasOne(d => d.WarehouseNavigation)
                    .WithMany(p => p.WarehouseItems)
                    .HasForeignKey(d => d.Warehouse)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WarehouseItem_FK_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
