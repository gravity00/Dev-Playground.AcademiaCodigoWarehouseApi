using Microsoft.EntityFrameworkCore;

namespace AcademiaCodigoWarehouseApi.Database {
    public class WarehouseContext : DbContext {
        public WarehouseContext (DbContextOptions<WarehouseContext> options) : base (options) {
            
        }

        protected override void OnModelCreating (ModelBuilder builder) {
            builder.Entity<ProductEntity> (cfg => {
                cfg.ToTable ("Products");

                cfg.HasKey (e => e.Id);
                cfg.HasIndex (e => e.Code).IsUnique ();
                cfg.HasIndex (e => e.Name).IsUnique ();

                cfg.Property (e => e.Id).IsRequired ().ValueGeneratedOnAdd ();
                cfg.Property (e => e.Code).IsRequired ().HasMaxLength (5);
                cfg.Property (e => e.Name).IsRequired ().HasMaxLength (128);
                cfg.Property (e => e.Description).HasMaxLength (1024);
                cfg.Property (e => e.Price).IsRequired ();

                cfg.Property (e => e.CreatedOn).IsRequired ();
                cfg.Property (e => e.CreatedBy).IsRequired ().HasMaxLength (128);
                cfg.Property (e => e.UpdatedOn).IsRequired ();
                cfg.Property (e => e.UpdatedBy).IsRequired ().HasMaxLength (128);
                cfg.Property (e => e.DeletedOn);
                cfg.Property (e => e.DeletedBy).HasMaxLength (128);

                cfg.Property (e => e.Version).IsRequired ().IsConcurrencyToken ();

                cfg.HasMany (e => e.StockMovements).WithOne (e => e.Product)
                    .HasForeignKey (e => e.ProductId).IsRequired ();
            });

            builder.Entity<StockMovementEntity> (cfg => {
                cfg.ToTable ("StockMovements");

                cfg.HasKey (e => e.Id);

                cfg.Property (e => e.Id).IsRequired ().ValueGeneratedOnAdd ();
                cfg.Property (e => e.Price).IsRequired ();
                cfg.Property (e => e.Quantity).IsRequired ();

                cfg.Property (e => e.CreatedOn).IsRequired ();
                cfg.Property (e => e.CreatedBy).IsRequired ().HasMaxLength (128);

                /*  //  como está mapeado em ProductEntity, é desnecessário
                cfg.HasOne (e => e.Product).WithMany (e => e.StockMovements)
                    .HasForeignKey (e => e.ProductId).IsRequired ();
                //*/
            });
        }
    }
}