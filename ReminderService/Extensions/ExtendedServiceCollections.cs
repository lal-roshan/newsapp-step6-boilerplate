using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ExtendedServiceCollections
    {
        /// <summary>
        /// Method for adding authentication based on JWT to the services based on token data
        /// </summary>
        /// <param name="services">The services collection to which authentication is to be configured</param>
        /// <param name="tokenData">The configuration section representing the token data</param>
        /// <returns>Service collection configured with JWT authentication</returns>
        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services,
            IConfigurationSection tokenData)
        {
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
            return services;
        }

        /// <summary>
        /// Method for adding swagger to services with JWT authentication support
        /// </summary>
        /// <param name="services">The services collection to which swagger is to be configured</param>
        /// <returns>Service collection configured with Swagger</returns>
        public static IServiceCollection AddJWTSwagger(this IServiceCollection services)
        {
            /// Enable Swagger   
            services.AddSwaggerGen(swagger =>
            {
                ///This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Reminder API with JWT",
                    Description = "Reminder API"
                });
                /// To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = @"JWT Authorization header using the Bearer scheme.
                                    Enter 'Bearer' [space] and then your token in the text input below.
                                    Example: \Bearer 12345abcdef\",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });
            return services;
        }
    }
}
