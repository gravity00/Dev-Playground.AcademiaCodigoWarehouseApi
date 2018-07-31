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

            using(var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope()){
                SeedDummyData(scope.ServiceProvider.GetRequiredService<WarehouseContext>());
            }
        }

        private void SeedDummyData(WarehouseContext ctx){
            var dummyData = new []{
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

            foreach(var p in dummyData){
                ctx.Add(p);
            }

            ctx.SaveChanges();
        }
    }
}