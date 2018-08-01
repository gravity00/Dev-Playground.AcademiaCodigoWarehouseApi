using System.ComponentModel.DataAnnotations;

namespace AcademiaCodigoWarehouseApi.Controllers.StockMovements
{
    public class CreateStockmovementModel {
        [Required]
        public int Quantity { get; set; }
    }
}