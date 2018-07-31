using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademiaCodigoWarehouseApi.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiaCodigoWarehouseApi {
    public class Startup {
        public void ConfigureServices (IServiceCollection services) {
            services.AddDbContext<WarehouseContext> (builder => {
                builder.UseInMemoryDatabase("Warehouse");
            });

            services.AddMvc ();
        }

        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseMvcWithDefaultRoute ();
        }
    }
}