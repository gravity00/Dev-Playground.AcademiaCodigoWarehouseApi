using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaCodigoWarehouseApi.Controllers.Products
{
    public interface IProductsEndpoint {
        IReadOnlyCollection<ProductSearchItemModel> Search (
            string code, string name, decimal? minPrice, decimal? maxPrice, bool? isActive,
            int skip = 0, int take = 20
        );

        IActionResult Create (CreateProductModel model);

        IActionResult Get(string code);

        IActionResult Update (long id, UpdateProductModel model);

        IActionResult Delete (long id, DeleteProductModel model);
    }
}