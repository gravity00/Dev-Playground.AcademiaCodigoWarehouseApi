using System;
using System.Collections.Generic;
using System.Linq;
using AcademiaCodigoWarehouseApi.Database;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaCodigoWarehouseApi.Controllers.Products {
    [Route ("api/products")]
    public class ProductsController : ControllerBase, IProductsEndpoint {

        private static readonly List<ProductEntity> MockProducts = new List<ProductEntity>{
                new ProductEntity {
                    Id = 1,
                    Code = "11111",
                    Name = "Bola de Praia",
                    Description = "Bola de praia maravilhosa",
                    Price = 10.5m,
                    StockMovements ={
                        new StockMovementEntity{
                            Price = 10.5m,
                            Quantity = -2,
                            CreatedOn = DateTimeOffset.Now.AddDays(-2),
                            CreatedBy = "joao.simoes"
                        },
                        new StockMovementEntity{
                            Price = 9m,
                            Quantity = 10,
                            CreatedOn = DateTimeOffset.Now.AddDays(-3),
                            CreatedBy = "joao.simoes"
                        }
                    },
                    CreatedOn = DateTimeOffset.Now.AddDays(-3),
                    CreatedBy = "joao.simoes",
                    UpdatedOn = DateTimeOffset.Now.AddMinutes (-32),
                    UpdatedBy = "joao.simoes"
                },
                new ProductEntity {
                    Id = 2,
                    Code = "22222",
                    Name = "Bola de Futebol",
                    Description = "Esta bola nem é assim tão boa",
                    Price = 50m,
                    CreatedOn = DateTimeOffset.Now.AddDays(-3),
                    CreatedBy = "joao.simoes",
                    UpdatedOn = DateTimeOffset.Now.AddHours (-5),
                    UpdatedBy = "renato.verissimo"
                }
        };

        [Route ("search"), HttpGet]
        public IReadOnlyCollection<ProductSearchItemModel> Search (
            string code, string name, decimal? minPrice, decimal? maxPrice, bool? isActive,
            int skip = 0, int take = 20
        ) {
            var username = User.Identity.Name;

            var filterItems = MockProducts.Select(e => new ProductSearchItemModel{
                Code = e.Code,
                Name = e.Name,
                Price = e.Price,
                CurrentStock = e.StockMovements.Sum(m => m.Quantity),
                IsActive = e.DeletedOn == null,
                UpdatedOn = e.UpdatedOn,
                UpdatedBy = e.UpdatedBy
            });

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

        [Route ("create"), HttpPost]
        public CreateProductResultModel Create ([FromBody] CreateProductModel model) {
            if (ModelState.IsValid) {

            }

            throw new NotImplementedException ();
        }
    }
}