﻿using Microsoft.AspNetCore.Mvc;
using Services;
using Shared.ErrorModels;
using Persistence;
using Domain.Contracts;
using Store.G00.Api.Middlewares;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Persistence.Data;
using Persistence.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Store.G00.Api.Extensions
{
    public static class Extension
    {

        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddBuiltInServices();

            services.AddSwaggerServices();

            services.AddInfrastructureServices(configuration);

            services.AddApplicationServices(configuration);
            services.AddIdentityServices();

            services.ConfigureServices();
            services.ConfigureJwtServices(configuration);

            services.AddCors(config =>
            {
                config.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    //options.AllowAnyOrigin();
                    options.WithOrigins("http://localhost:4200");
                });
            });

            return services;
        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {

            services.AddControllers();
            return services;
        }
        
        private static IServiceCollection ConfigureJwtServices(this IServiceCollection services,IConfiguration configuration)
        {

            var JwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidIssuer = JwtOptions.Issuer,
                    ValidAudience = JwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey)),

                };

            });

            return services;
        }

        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {

            services.AddIdentity<AppUser,IdentityRole>()
                    .AddEntityFrameworkStores<StoreIdenityDbContext>();
            return services;
        }


        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {

            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    var errrors = ActionContext.ModelState.Where(m => m.Value.Errors.Any())
                                    .Select(m => new ValidationError()
                                    {
                                        Field = m.Key,
                                        Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                                    });

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errrors
                    };

                    return new BadRequestObjectResult(response);
                };
            });



            return services;
        }

        public static async Task<WebApplication> ConfigureMiddlewares(this WebApplication app)
        {

            await app.InitializeDatabaseAsync();

            app.UseGlobalErrorHandling();

           
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseCors("MyPolicy");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            return app;
        }

        private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {

            #region Data Seeding

            using var scope = app.Services.CreateScope();

            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // Allow CLR To Make Object From DbInitializer

            await dbInitializer.InitializeAsync();
            await dbInitializer.InitializeIdentityAsync();

            #endregion

            return app;
        }

        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            return app;
        }

    }
}
