using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using TestApi.Helpers;
using TestApi.Utils;
using LinqToDB.Data;
using Database;

namespace TestApi
{
    public class Startup
    {
        public IConfiguration _config { get; }
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
            DataConnection.DefaultSettings = new DbConnectionSettings(configuration.GetConnectionString("ConectionDb"));
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = _config.GetSection("AppSettings").Get<AppSettings>();
            //services.AddSingleton(appSettings);

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new TrimStringConverter());
                });
            services.AddMemoryCache(); // rejestracja w IoC MemoryCache

            services.AddLogging();
            
            services.AddScoped<IDbContextFactory, DbContextFactory>();

            services.AddAutoMapper(typeof(Startup));
            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(new DeveloperExceptionPageOptions { SourceCodeLineCount = 2 });
            }

            app.UseMiddleware<ResponseTimeMiddlewareAsync>();
            app.UseMiddleware<ErrorLoggingMiddlewareAsync>();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.ConfigureSwagger(provider);
        }
    }
}
