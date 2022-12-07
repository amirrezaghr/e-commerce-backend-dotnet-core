using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyEshop.Models;

namespace MyEshop.Data
{
    public class MyEshopContext:DbContext
    {
        public MyEshopContext(DbContextOptions<MyEshopContext> options):base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryToProduct> CategoryToProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryToProduct>()
                .HasKey(t => new { t.ProductId, t.CategoryId });

            //modelBuilder.Entity<Product>(
            //    p =>
            //    {
            //        p.HasKey(w => w.Id);
            //        p.OwnsOne<Item>(w => w.Item);
            //        p.HasOne<Item>(w => w.Item).WithOne(w => w.Product)
            //        .HasForeignKey<Item>(w => w.Id);
            //    }
            //    );

            modelBuilder.Entity<Item>(
                i =>
                {
                    i.Property(w => w.Price).HasColumnType("Money");
                    i.HasKey(w => w.Id);
                }
                );

            #region Seed Data Category
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    Name = "Asp.net Core",
                    Description = "Asp.Net Core Descript"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Python Course",
                    Description = "Python Descript"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Csharp Course",
                    Description = "csharp course Descript"
                }
                ); ;

            modelBuilder.Entity<Item>().HasData(
                new Item()
                {
                    Id = 1,
                    Price = 546.8M,
                    QuantityInStock = 5
                },
                new Item()
                {
                    Id = 2,
                    Price = 33.5M,
                    QuantityInStock = 8
                },
                new Item()
                {
                    Id = 3,
                    Price = 2500,
                    QuantityInStock = 3
                }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    ItemId = 1,
                    Name = "آموزش پروژه محور asp.net core",
                    Description = "آموزش بک اند با Asp.net core به همراه پروژه فروشگاهی"
                },
                new Product()
                {
                    Id = 2,
                    ItemId = 2,
                    Name = "آموزش مقدماتی سی شارپ",
                    Description = "آموزش مقدماتی سی شارپ به همراه پروژه عملی مدیریت کتابخانه در پایان ترم"
                },
                new Product()
                {
                    Id = 3,
                    ItemId = 3,
                    Name = "آموزش پایتون برای اهداف هوش مصنوعی",
                    Description = "آموزشی برای افرادی که قصد دارند در آینده متخصص هوش مصنوعی باشند"
                }
                );

            modelBuilder.Entity<CategoryToProduct>().HasData(
                new CategoryToProduct() { CategoryId = 1, ProductId = 1},
                new CategoryToProduct() { CategoryId = 2, ProductId = 1 },
                new CategoryToProduct() { CategoryId = 3, ProductId = 1 },
                new CategoryToProduct() { CategoryId = 1, ProductId = 2 },
                new CategoryToProduct() { CategoryId = 2, ProductId = 2 },
                new CategoryToProduct() { CategoryId = 3, ProductId = 2 },
                new CategoryToProduct() { CategoryId = 1, ProductId = 3 },
                new CategoryToProduct() { CategoryId = 2, ProductId = 3 },
                new CategoryToProduct() { CategoryId = 3, ProductId = 3 }
                );
                #endregion
            base.OnModelCreating(modelBuilder);
        }
    }
}
