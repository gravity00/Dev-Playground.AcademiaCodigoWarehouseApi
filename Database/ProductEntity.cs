using System;
using System.Collections.Generic;

namespace AcademiaCodigoWarehouseApi.Database
{
    public class ProductEntity {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public long Version { get; set; }
        public ICollection<StockMovementEntity> StockMovements { get; set; }

        public ProductEntity () {
            StockMovements = new List<StockMovementEntity> ();
        }
    }
}