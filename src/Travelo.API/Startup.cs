using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Travelo.API.Middleware;
using Travelo.Core.Domain.Providers;
using Travelo.Core.Mappers;
using Travelo.Core.Repositories;
using Travelo.Core.Services;
using Travelo.DataStore;

namespace Travelo.API
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
            services.AddControllers();
            services.Configure<SqlSettings>(Configuration.GetSection("ConnectionString"));
            services.AddDbContext<TraveloDataContext>();
            services.AddSingleton<IDateTimeOffsetProvider, DateTimeOffsetProvider>();
            services.AddSingleton<ITraveloMapper, TraveloMapper>();
            services.AddScoped<ICustomerRepository, SqlCustomerRepository>();
            services.AddScoped<ITripRepository, SqlTripRepository>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddEntityFrameworkSqlServer().AddEntityFrameworkInMemoryDatabase();
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsDev",
                    builder => builder.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials().WithExposedHeaders("Location"));
                options.AddPolicy(
                    "CorsProd",
                    builder => builder.WithOrigins("https://traveloportal.azurewebsites.net")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials().WithExposedHeaders("Location"));
            });
            services.AddMvcCore().AddNewtonsoftJson();//Got some issue on my Mac with System.Text.Json - cannot see 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("CorsDev");
            }
            else
            {
                app.UseCors("CorsProd");
            }

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}