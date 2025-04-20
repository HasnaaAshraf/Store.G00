
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Services;
using ServicesAbstractions;
using Shared.ErrorModels;
using Store.G00.Api.Extensions;
using Store.G00.Api.Middlewares;
using AssemblyMapping = Services.AssemblyReference;  // Make this Because There Is Two AssemblyReference , So We Special One By Elyas Name

namespace Store.G00.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.RegisterAllServices(builder.Configuration);


            var app = builder.Build();

            await app.ConfigureMiddlewares();

            app.Run();
        }
    }
}
