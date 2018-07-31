using System.ComponentModel.DataAnnotations;

namespace AcademiaCodigoWarehouseApi.Controllers.Products {
    public class DeactivateProductModel {
        [Required]
        public long Version { get; set; }
    }
}