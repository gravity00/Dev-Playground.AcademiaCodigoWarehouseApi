using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaCodigoWarehouseApi.Controllers.Products
{
    [Route ("api/products")]
    public class ProductsController : ControllerBase, IProductsEndpoint {

        [Route ("search"), HttpGet]
        public IReadOnlyCollection<ProductSearchItemModel> Search (
            string code, string name, decimal? minPrice, decimal? maxPrice, bool? isActive,
            int skip = 0, int take = 20
        ) {
            var username = User.Identity.Name;

            var result = new [] {
                new ProductSearchItemModel {
                Code = "11111",
                Name = "Bola de Praia",
                Price = 10.5m,
                CurrentStock = 50,
                IsActive = true,
                UpdatedOn = DateTimeOffset.Now.AddMinutes (-32),
                UpdatedBy = "joao.simoes"
                },
                new ProductSearchItemModel {
                Code = "22222",
                Name = "Bola de Futebol",
                Price = 50m,
                CurrentStock = 10,
                IsActive = true,
                UpdatedOn = DateTimeOffset.Now.AddHours (-5),
                UpdatedBy = "renato.verissimo"
                }
            };

            IEnumerable<ProductSearchItemModel> filterItems = result;

            if (!string.IsNullOrWhiteSpace (code)) {
                filterItems = filterItems
                    .Where (e => e.Code.Contains (code.Trim (), StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace (name)) {
                filterItems = filterItems
                    .Where (e => e.Name.Contains (name.Trim (), StringComparison.InvariantCultureIgnoreCase));
            }

            if (minPrice.HasValue) {
                filterItems = filterItems
                    .Where (e => e.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue) {
                filterItems = filterItems
                    .Where (e => e.Price <= maxPrice.Value);
            }

            if (isActive.HasValue) {
                filterItems = filterItems
                    .Where (e => e.IsActive == isActive.Value);
            }

            return filterItems.AsPage (skip, take).ToList ();
        }
    }
}