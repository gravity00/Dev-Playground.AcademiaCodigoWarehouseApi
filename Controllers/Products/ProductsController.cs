using System;
using System.Collections.Generic;
using System.Linq;
using AcademiaCodigoWarehouseApi.Database;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaCodigoWarehouseApi.Controllers.Products {
    [Route ("api/products")]
    public class ProductsController : Controller, IProductsEndpoint {

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
        public IActionResult Create ([FromBody] CreateProductModel model) {
            if (ModelState.IsValid) {

                if(MockProducts.Any(e => e.Code.Equals(model.Code.Trim(), StringComparison.InvariantCultureIgnoreCase))){
                    return Conflict(new {
                        Message="Código duplicado"
                    });
                }

                if(MockProducts.Any(e => e.Name.Equals(model.Name.Trim(), StringComparison.InvariantCultureIgnoreCase))){
                    return Conflict(new {
                        Message="Nome duplicado"
                    });
                }

                var now = DateTimeOffset.Now;
                var username = User.Identity.Name;
                var newId = MockProducts.Max(e => e.Id) + 1;

                MockProducts.Add(new ProductEntity{
                    Id = newId,
                    Code = model.Code,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    CreatedOn = now,
                    CreatedBy = username,
                    UpdatedOn = now,
                    UpdatedBy = username,
                });

                return Json(new CreateProductResultModel{
                    Id = newId
                });
            }

            return UnprocessableEntity(new {
                Message="Entidade com dados inválidos",
                ModelState = ModelState.Select(e => new {
                    Key = e.Key,
                    Value = e.Value.Errors
                })
            });
        }

        [Route("{code}"), HttpGet]
        public IActionResult Get(string code)
        {
            var product = MockProducts
                .SingleOrDefault(e => e.Code.Equals(code.Trim(), StringComparison.InvariantCultureIgnoreCase));
            
            if(product == null)
                return NotFound();
            return Json(new ProductModel{
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CurrentStock = product.StockMovements.Sum(m => m.Quantity),
                CreatedOn = product.CreatedOn,
                CreatedBy = product.CreatedBy,
                UpdatedOn = product.UpdatedOn,
                UpdatedBy = product.UpdatedBy,
                DeletedOn = product.DeletedOn,
                DeletedBy = product.DeletedBy,
                Version = product.Version
            });
        }
    }
}