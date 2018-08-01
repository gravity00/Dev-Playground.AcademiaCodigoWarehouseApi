using System;
using System.Collections.Generic;

namespace AcademiaCodigoWarehouseApi.Controllers.StockMovements
{
    public interface IStockMovementsEndpoint {
        IReadOnlyCollection<StockMovementModel> Search (
            string productCode, string productName, DateTimeOffset? minDate, DateTimeOffset? maxDate,
            int skip = 0, int take = 20
        );

        StockMovementActionResultModel Create(string productCode, CreateStockmovementModel model);
    }
}