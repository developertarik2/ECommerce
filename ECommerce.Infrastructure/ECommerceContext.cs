using ECommerce.Models.Entities;
using ECommerce.Models.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure
{
   public class ECommerceContext : DbContext
    {
        public ECommerceContext(DbContextOptions<ECommerceContext> options)
               : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        public DbSet<CustomerCart> CustomerCarts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            //{
            //    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //    {
            //        var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));
            //        //var dateTimeProperties = entityType.ClrType.GetProperties()
            //        //    .Where(p => p.PropertyType == typeof(DateTimeOffset));

            //        foreach (var property in properties)
            //        {
            //            modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
            //        }

            //        //foreach (var property in dateTimeProperties)
            //        //{
            //        //    modelBuilder.Entity(entityType.Name).Property(property.Name)
            //        //        .HasConversion(new DateTimeOffsetToBinaryConverter());
            //        //}
            //    }
            //}
        }
    }
}
