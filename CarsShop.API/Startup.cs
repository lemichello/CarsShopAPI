using AutoMapper;
using CarsShop.API.Configuration.MapperProfiles;
using CarsShop.DAL;
using CarsShop.DAL.Repositories.Abstraction;
using CarsShop.DAL.Repositories.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarsShop.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(i => i.AddPolicy("CorsPolicy",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()));

            services.AddDbContext<DbContext, EfContext>(opt => opt.UseSqlServer(Configuration["ConnectionString"]));
            services.AddTransient(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
