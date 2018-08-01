using System;
using System.Collections.Generic;
using System.Linq;
using AcademiaCodigoWarehouseApi.Database;
using AcademiaCodigoWarehouseApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaCodigoWarehouseApi.Controllers.Products {
    [Route ("api/products")]
    public class ProductsController : Controller, IProductsEndpoint {

        private readonly WarehouseContext _ctx;

        public ProductsController(WarehouseContext ctx){
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        [Route ("search"), HttpGet]
        public IReadOnlyCollection<ProductSearchItemModel> Search (
            string code, string name, decimal? minPrice, decimal? maxPrice, bool? isActive,
            int skip = 0, int take = 20
        ) {
            var filterItems = _ctx.Set<ProductEntity>().Select(e => new ProductSearchItemModel{
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

            return filterItems
                .OrderBy(e => e.Name)
                .ThenBy(e => e.Code)
                .AsPage (skip, take).ToList ();
        }

        [Route ("create"), HttpPost]
        public CreateProductResultModel Create ([FromBody] CreateProductModel model) {
            if(!ModelState.IsValid){
                throw new ValidationException(ModelState);
            }

            var productsSet = _ctx.Set<ProductEntity>();

            if(productsSet.Any(e => e.Code.Equals(model.Code.Trim(), StringComparison.InvariantCultureIgnoreCase))){
                throw new BusinessException("Código duplicado");
            }

            if(productsSet.Any(e => e.Name.Equals(model.Name.Trim(), StringComparison.InvariantCultureIgnoreCase))){
                throw new BusinessException("Nome duplicado");
            }
            
            var now = DateTimeOffset.Now;
            var username = this.GetUserName();

            var product = new ProductEntity{
                Code = model.Code,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CreatedOn = now,
                CreatedBy = username,
                UpdatedOn = now,
                UpdatedBy = username,
                Version = Guid.NewGuid()
            };
            productsSet.Add(product);

            _ctx.SaveChanges();

            return new CreateProductResultModel{
                Id = product.Id,
                Version = product.Version
            };
        }

        [Route("{code}"), HttpGet]
        public ProductModel Get(string code)
        {
            var product = _ctx.Set<ProductEntity>()
                .SingleOrDefault(e => e.Code.Equals(code.Trim(), StringComparison.InvariantCultureIgnoreCase));
            
            if(product == null)
                throw new NotFoundException();
            return new ProductModel{
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
            };
        }

        [Route("update/{id}"), HttpPost]
        public ProductActionResultModel Update (long id, [FromBody] UpdateProductModel model){
            if(!ModelState.IsValid){
                throw new ValidationException(ModelState);
            }
            
            var productsSet = _ctx.Set<ProductEntity>();

            var product = productsSet.SingleOrDefault(e => e.Id == id);
            if(product == null){
                throw new NotFoundException();
            }

            if(product.Version != model.Version){
                throw new BusinessException("Entidade foi alterada por outro utilizador");
            }

            var modelCode = model.Code.Trim();
            if(!product.Code.EqualsOrdinalIgnoreCase(modelCode) && productsSet.Any(e => e.Code.EqualsOrdinalIgnoreCase(modelCode))){
                throw new BusinessException("Código duplicado");
            }

            var modelName = model.Name.Trim();
            if(!product.Name.EqualsOrdinalIgnoreCase(modelName) && productsSet.Any(e => e.Name.EqualsOrdinalIgnoreCase(modelName))){
                throw new BusinessException("Nome duplicado");
            }

            product.Code = model.Code;
            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.UpdatedOn = DateTimeOffset.Now;
            product.UpdatedBy = this.GetUserName();
            product.Version = Guid.NewGuid();

            productsSet.Update(product);

            _ctx.SaveChanges();

            return new ProductActionResultModel{
                Version = product.Version
            };
        }

        [Route("deactivate/{id}"), HttpPost]
        public ProductActionResultModel Deactivate (long id, [FromBody] DeactivateProductModel model){
            if(!ModelState.IsValid){
                throw new ValidationException(ModelState);
            }

            var product = _ctx.Set<ProductEntity>().SingleOrDefault(e => e.Id == id);
            if(product == null){
                throw new NotFoundException();
            }

            if(product.Version != model.Version){
                throw new BusinessException("Entidade foi alterada por outro utilizador");
            }

            product.DeletedOn = product.UpdatedOn = DateTimeOffset.Now;
            product.DeletedBy = product.UpdatedBy = this.GetUserName();
            product.Version = Guid.NewGuid();

            _ctx.SaveChanges();

            return new ProductActionResultModel{
                Version = product.Version
            };
        }

        [Route("activate/{id}"), HttpPost]
        public ProductActionResultModel Activate(long id, [FromBody] ActivateProductModel model){
            if(!ModelState.IsValid){
                throw new ValidationException(ModelState);
            }

            var product = _ctx.Set<ProductEntity>().SingleOrDefault(e => e.Id == id);
            if(product == null){
                throw new NotFoundException();
            }

            if(product.Version != model.Version){
                throw new BusinessException("Entidade foi alterada por outro utilizador");
            }

            product.DeletedOn = null;
            product.DeletedBy = null;
            product.UpdatedOn = DateTimeOffset.Now;
            product.UpdatedBy = this.GetUserName();
            product.Version = Guid.NewGuid();

            _ctx.SaveChanges();

            return new ProductActionResultModel{
                Version = product.Version
            };
        }
    }
}