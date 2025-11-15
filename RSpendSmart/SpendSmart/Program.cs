using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;
using SQLitePCL;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace SpendSmart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            Batteries.Init();

            // builder.Services.AddDbContext<SpendSmartDbContext>(options =>
            // options.UseInMemoryDatabase("SpendSmartDb")
            //    );

            builder.Services.AddDbContext<SpendSmartDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("SpendSmartDb")
                ));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}
