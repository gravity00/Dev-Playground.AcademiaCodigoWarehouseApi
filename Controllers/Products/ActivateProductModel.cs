using System.ComponentModel.DataAnnotations;

namespace AcademiaCodigoWarehouseApi.Controllers.Products {
    public class ActivateProductModel {
        [Required]
        public long Version { get; set; }
    }
}