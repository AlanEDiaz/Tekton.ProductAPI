using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using MediatR;
using System.Reflection;
using System.Threading.Tasks;
using Tekton.ProductAPI.Infrastructure.Logger;
using Tekton.ProductAPI.Infrastructure;
using Tekton.ProductAPI.Models;
using Tekton.ProductAPI.Cqrs.Handlers.Queries.GetAllProducts;
using Tekton.ProductAPI.Cqrs.Handlers.Queries.GetProduct;
using Tekton.ProductAPI.Cqrs.Handlers.Commands.CreateProduct;
using Tekton.ProductAPI.Cqrs.Handlers.Commands.UpdateProduct;
using Microsoft.OpenApi.Models;
using Tekton.ProductAPI.Services;
using Tekton.ProductAPI.Infrastructure.Repositories;
using Tekton.ProductAPI.Infrastructure.Caching;
using Tekton.ProductAPI.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using LiteDB;

namespace Tekton.ProductAPI
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddControllers();
            services.AddHttpClient();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddSingleton<IAPILogger, APILogger>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICommandRepository, CommandRepository>();
            services.AddScoped<ProductService>();
            services.AddSingleton<IProductCache, ProductCache>();

            services.AddDbContext<MainDbContext>(options =>
            {
                //to use Sql database instead of MySql must change this line UseSql(Configuration.GetConnectionString("LocalConnection"))
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection")))
                        .LogTo(Console.WriteLine, LogLevel.Information)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
            });


            services.AddTransient<IRequestHandler<GetAllProductsQuery, IEnumerable<GetAllProductsQueryResult>>, GetAllProductsQueryHandler>();
            services.AddTransient<IRequestHandler<GetProductQuery, GetProductQueryResult>, GetProductQueryHandler>();



            services.AddTransient<IRequestHandler<CreateProductCommand, Guid>, CreateProductCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateProductCommand, bool>, UpdateProductCommandHandler>();



            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(AOption =>
            {
                AOption.SwaggerDoc("v1", new OpenApiInfo { Title = "Challange Api", Version = "v1" });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();



            app.UseSwagger();
            app.UseSwaggerUI(AOption
                => AOption.SwaggerEndpoint("/swagger/v1/swagger.json", "Challange Api version 1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
