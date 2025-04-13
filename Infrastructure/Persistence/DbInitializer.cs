using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _context;

        public DbInitializer(StoreDbContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            // Create Database If It Doesn't Exist And Apply To Any Pending Migration (Lsa Msma3tesh El DataBase => Update Database)
           
            if(_context.Database.GetPendingMigrations().Any())
            {
                await _context.Database.MigrateAsync();
            }

            // Data Seeding 

            // Seeding ProductTypes From Json File 

            if (!_context.ProductTypes.Any())
            {
                // 1. Read All Data Types From Types Json File 

                 var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Seeding\types.json");

                // 2. Transform String To C# Objects (List<ProductTypes>)

                 var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                // 3. Add Data To Database

                if(types is not null && types.Any())
                {
                    await _context.ProductTypes.AddRangeAsync(types);
                    await _context.SaveChangesAsync();
                }
            }


            // Seeding ProductBrands From Json File 

            if (!_context.ProductBrands.Any())
            {
                // 1. Read All Data Brands From Brands Json File 

                 var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Seeding\brands.json");

                // 2. Transform String To C# Objects (List<ProductBrands>)

                 var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                // 3. Add Data To Database

                if(brandsData is not null && brands.Any())
                {
                    await _context.ProductBrands.AddRangeAsync(brands);
                    await _context.SaveChangesAsync();
                }

            }


            // Seeding Products From Json File 
           
            if(! _context.Products.Any())
            {
                // 1. Read All Data Products From Types Json File 

                 var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Seeding\products.json");

                // 2. Transform String To C# Objects (List<Products>)

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                // 3. Add Data To Database

               if(products is not null && products.Any())
                {
                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }

            }

        }
    }
}


// \Infrastructure\Persistence\Seeding\types.json

// \Infrastructure\Persistence\Seeding\brands.json

// \Infrastructure\Persistence\Seeding\products.json