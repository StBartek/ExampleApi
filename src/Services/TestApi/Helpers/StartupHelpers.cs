using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using TestApi.Swagger;

namespace TestApi.Helpers
{
    public static class StartupHelpers
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                });
            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    options.IncludeXmlComments(XmlCommentsFilePath);

                    //options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                    //{
                    //    Name = "Authorization",
                    //    Type = SecuritySchemeType.Http,
                    //    Scheme = "basic",
                    //    In = ParameterLocation.Header,
                    //    Description = "Basic Authorization header using the Bearer scheme."
                    //});

                    //options.AddSecurityRequirement(
                    //    new OpenApiSecurityRequirement {
                    //        {
                    //            new OpenApiSecurityScheme
                    //            {
                    //                Reference = new OpenApiReference
                    //                {
                    //                    Type = ReferenceType.SecurityScheme,
                    //                    Id = "basic"
                    //                }
                    //            },
                    //            new string[] {}
                    //        }
                    //});
                });
        }

        public static void ConfigureSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        options.RoutePrefix = string.Empty;
                    }
                });
        }

        private static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
}
