using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademiaCodigoWarehouseApi.Database;
using AcademiaCodigoWarehouseApi.Middleware.ApiErrorHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace AcademiaCodigoWarehouseApi {
    public class Startup {
        public void ConfigureServices (IServiceCollection services) {
            services.AddDbContext<WarehouseContext> (builder => {
                builder.UseMySql("server=localhost;uid=warehouse-api;pwd=123456;database=Warehouse;oldguids=true");
            });

            services.AddMvc ();
        }

        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseMiddleware<ApiErrorHandlerMiddleware>(Options.Create(new ApiErrorHandlerOptions{
                IndentJson = true
            }));

            app.UseMvcWithDefaultRoute ();

            using(var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope()){
                scope.ServiceProvider.GetRequiredService<WarehouseContext>().Database.EnsureCreated();
            }
        }
    }
}