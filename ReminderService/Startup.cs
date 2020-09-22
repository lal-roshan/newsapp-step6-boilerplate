using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ReminderService.Models;
using ReminderService.Repository;
using ReminderService.Services;
using System;
using System.Text;

namespace ReminderService
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
            services.AddSingleton(s => new ReminderContext(Configuration));
            services.AddScoped<IReminderService, Services.ReminderService>();
            services.AddScoped<IReminderRepository, ReminderRepository>();
            services.AddControllers();

            ///reading token payload related data from appsettings
            var tokenData = Configuration.GetSection("TokenData");

            ///add options for authentication
            services.AddAuthentication(
                options =>
                {
                    ///Provide default and challenge schema
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
                ).AddJwtBearer(
                    ///Mention parameters that are to be validated
                    o => o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = tokenData["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = tokenData["Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenData["SecretKey"])),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }
                );
            //provide options for Database Context to Register Dependencies
            //Register all dependencies here
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
