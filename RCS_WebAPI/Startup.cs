using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.OpenApi.Models;

namespace RCS_WebAPI
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
            try
            {
                string Connection_String = Configuration.GetConnectionString("DBConnection");
                GlobalProperties.DBConnection = Connection_String;
                GlobalProperties.FromEmailAddress = Configuration["EmailSettings:FromEmailAddress"];
                GlobalProperties.FromEmailName = Configuration["EmailSettings:FromEmailName"];
                GlobalProperties.FromEmailPassword = Configuration["EmailSettings:FromEmailPassword"];
                GlobalProperties.OTPExpiryMinutes = Configuration["EmailSettings:OTPExpiryMinutes"];
                GlobalProperties.RandomOTP = Guid.NewGuid().ToString("n").Substring(0, 8) + "@DCL";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            #region Swagger Configure

            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "RCS WebAPI",
                    Description = "ASP.NET Core Web API fetch Data from Database using Entity Framework Core.",
                });
            });

            #endregion

            //Commented By Imran Khan to replace with Swagger.
            //services.AddControllers();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region Swagger Configure
            //Added By Imran Khan to use Swagger.

            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RCS WebAPI");
                c.ConfigObject.AdditionalItems.Add("syntaxHighlight", false); //Turns off syntax highlight which causing performance issues...
                c.ConfigObject.AdditionalItems.Add("theme", "agate"); //Reverts Swagger UI 2.x  theme which is simpler not much performance benefit...
                c.RoutePrefix = string.Empty;
            });

            #endregion

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
    public static class GlobalProperties
    {
        public static string DBConnection { get; set; }
        public static string FromEmailAddress { get; set; }
        public static string FromEmailName { get; set; }
        public static string FromEmailPassword { get; set; }
        public static string OTPExpiryMinutes { get; set; }
        public static string RandomOTP { get; set; }
    }
}