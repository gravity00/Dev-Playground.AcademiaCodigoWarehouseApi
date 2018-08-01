using System.Collections.Generic;

namespace AcademiaCodigoWarehouseApi.Controllers.Products
{
    public interface IProductsEndpoint {
        IReadOnlyCollection<ProductSearchItemModel> Search (
            string code, string name, decimal? minPrice, decimal? maxPrice, bool? isActive,
            int skip = 0, int take = 20
        );

        CreateProductResultModel Create (CreateProductModel model);

        ProductModel Get(string code);

        ProductActionResultModel Update (long id, UpdateProductModel model);

        ProductActionResultModel Deactivate (long id, DeactivateProductModel model);

        ProductActionResultModel Activate (long id, ActivateProductModel model);
    }
}