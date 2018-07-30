using System;

namespace AcademiaCodigoWarehouseApi.Database
{
    public class StockMovementEntity {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public ProductEntity Product { get; set; }
    }
}