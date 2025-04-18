
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Services;
using ServicesAbstractions;

using AssemblyMapping = Services.AssemblyReference;  // Make this Because There Is Two AssemblyReference , So We Special One By Elyas Name

namespace Store.G00.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IDbInitializer, DbInitializer>(); // Allow DI of DbInitializer

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();

            builder.Services.AddAutoMapper(typeof(AssemblyMapping).Assembly);



            var app = builder.Build();

            #region Data Seeding

            using var scope = app.Services.CreateScope();

            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // Allow CLR To Make Object From DbInitializer

            await dbInitializer.InitializeAsync();

            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
