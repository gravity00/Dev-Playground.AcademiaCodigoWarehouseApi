using System;

namespace AcademiaCodigoWarehouseApi.Controllers.StockMovements
{
    public class StockMovementModel {
        public long Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}