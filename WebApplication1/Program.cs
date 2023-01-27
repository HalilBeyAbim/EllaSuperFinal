using Ella.BLL.Helpers;
using Ella.DAL.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata;

namespace EllaSuperFinal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMvc();
            builder.Services.AddControllersWithViews();

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    builder =>
                    {
                        builder.MigrationsAssembly(nameof(EllaSuper));
                    });

            });
            //builder.Services.AddDbContext<AppDbContext>(options =>
            //{
            //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            //    builder =>
            //    {
            //        builder.MigrationsAssembly("EllaSuperFinal");
            //    });
            //});

            Constants.RootPath = builder.Environment.WebRootPath;
            Constants.BlogPath = Path.Combine(Constants.RootPath, "assets","images", "blog");
            Constants.AboutPath = Path.Combine(Constants.RootPath, "assets", "images", "about");
            Constants.TeamPath = Path.Combine(Constants.RootPath, "assets", "images", "team");
            Constants.GalleryPath = Path.Combine(Constants.RootPath, "assets", "images", "gallery");


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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=dashboard}/{action=Index}/{id?}"
                    );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
            });

            app.Run();
        }
    }
}