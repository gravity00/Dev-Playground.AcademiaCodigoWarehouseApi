using System;
using System.ComponentModel.DataAnnotations;

namespace AcademiaCodigoWarehouseApi.Controllers.Products {
    public class UpdateProductModel {
        [Required]
        [MinLength (5)]
        [MaxLength (5)]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public Guid Version { get; set; }
    }
}