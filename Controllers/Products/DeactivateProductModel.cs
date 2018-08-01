using System;
using System.ComponentModel.DataAnnotations;

namespace AcademiaCodigoWarehouseApi.Controllers.Products {
    public class DeactivateProductModel {
        [Required]
        public Guid Version { get; set; }
    }
}