using ECommerce.Models.Entities;
using ECommerce.Models.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data
{
   public class ECommerceContextSeed
    {
        public static async Task SeedAsync(ECommerceContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!context.Brands.Any())
                {
                    var brandsData = File.ReadAllText(path+ @"/Data/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<Brand>>(brandsData);
                    foreach (var item in brands)
                    {
                        Brand model = new()
                        {
                            Name = item.Name
                        };
                        context.Brands.Add(model);
                    }
                        await context.SaveChangesAsync();
                }
                if (!context.Categories.Any())
                {
                    var typesData = File.ReadAllText(path+ @"/Data/SeedData/category.json");
                    var types = JsonSerializer.Deserialize<List<Category>>(typesData);
                    foreach (var item in types)
                    {
                        Category model = new()
                        {
                            Name = item.Name
                        };
                        context.Categories.Add(model);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.Products.Any())
                {
                    var productsData = File.ReadAllText(path+ @"/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    foreach (var item in products)
                    {
                        //Product model = new Product
                        //{
                        //    Name = item.Name,
                        //    Description = item.Description,
                        //    Price = item.Price,
                        //    PictureUrl = item.PictureUrl,
                        //    BrandId = item.BrandId,
                        //    CategoryId = item.CategoryId
                        //};
                        context.Products.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.DeliveryMethods.Any())
                {
                    var typesData = File.ReadAllText(path+ @"/Data/SeedData/delivery.json");
                    var types = JsonSerializer.Deserialize<List<DeliveryMethod>>(typesData);
                    foreach (var item in types)
                    {
                        DeliveryMethod model = new()
                        {
                            ShortName = item.ShortName,
                            DeliveryTime = item.DeliveryTime,
                            Description = item.Description,
                            Price = item.Price
                        };
                        context.DeliveryMethods.Add(model);
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ECommerceContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
