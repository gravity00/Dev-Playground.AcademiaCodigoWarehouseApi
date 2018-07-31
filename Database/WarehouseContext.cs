using Microsoft.EntityFrameworkCore;

namespace AcademiaCodigoWarehouseApi.Database
{
    public class WarehouseContext : DbContext
    {
        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options)
        {

        }
    }
}