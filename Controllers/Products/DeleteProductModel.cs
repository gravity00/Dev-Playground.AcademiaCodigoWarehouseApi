using System.ComponentModel.DataAnnotations;

namespace AcademiaCodigoWarehouseApi.Controllers.Products {
    public class DeleteProductModel {
        [Required]
        public long Version { get; set; }
    }
}