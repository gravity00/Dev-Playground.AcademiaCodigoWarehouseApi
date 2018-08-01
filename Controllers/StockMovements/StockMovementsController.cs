using System;
using System.Collections.Generic;
using System.Linq;
using AcademiaCodigoWarehouseApi.Database;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaCodigoWarehouseApi.Controllers.StockMovements {
    [Route ("api/stockmovements")]
    public class StockMovementsController : Controller, IStockMovementsEndpoint {

        private readonly WarehouseContext _ctx;

        public StockMovementsController(WarehouseContext ctx){
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        [Route ("search"), HttpGet]
        public IReadOnlyCollection<StockMovementModel> Search (
            string productCode, string productName, DateTimeOffset? minDate, DateTimeOffset? maxDate, 
            int skip = 0, int take = 20
        ) {
            var filterItems = _ctx.Set<StockMovementEntity>().Select(e => new StockMovementModel{
                Id = e.Id,
                ProductCode = e.Product.Code,
                ProductName = e.Product.Name,
                Price = e.Price,
                Quantity = e.Quantity,
                CreatedOn = e.CreatedOn,
                CreatedBy = e.CreatedBy
            });

            if(!string.IsNullOrWhiteSpace(productCode)){
                filterItems = filterItems.Where(e => e.ProductCode.Contains(productCode));
            }

            if(!string.IsNullOrWhiteSpace(productName)){
                filterItems = filterItems.Where(e => e.ProductName.Contains(productName));
            }

            if(minDate.HasValue){
                filterItems = filterItems.Where(e => e.CreatedOn >= minDate.Value);
            }

            if(maxDate.HasValue){
                filterItems = filterItems.Where(e => e.CreatedOn <= maxDate.Value);
            }

            return filterItems
                .OrderByDescending(e => e.CreatedOn)
                .ThenByDescending(e => e.Id)
                .AsPage (skip, take).ToList ();
        }
    }
}