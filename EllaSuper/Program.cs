using Ella.BLL.Helpers;
using Ella.Core.Entity;
using Ella.DAL.DAL;
using Ella.DAL.DAL.Data;
using EllaSuper.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EllaSuperFinal
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

            builder.Services.AddMvc();
            builder.Services.AddControllersWithViews();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
               builder =>
               {
                   builder.MigrationsAssembly("Ella.DAL");
               });
            });
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            builder.Services.Configure<AdminUser>(builder.Configuration.GetSection("AdminUser"));

            builder.Services.AddScoped<LayoutService>();
            
            Constants.RootPath = builder.Environment.WebRootPath;
            Constants.BlogPath = Path.Combine(Constants.RootPath, "assets","images", "blog");
            Constants.AboutPath = Path.Combine(Constants.RootPath, "assets", "images", "about");
            Constants.TeamPath = Path.Combine(Constants.RootPath, "assets", "images", "team");
            Constants.GalleryPath = Path.Combine(Constants.RootPath, "assets", "images", "gallery");
            Constants.ProductPath = Path.Combine(Constants.RootPath, "assets", "images", "product");


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
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var dataInitializer = new DataIntializer(serviceProvider);
                await dataInitializer.SeedData();
            }

            app.UseRouting();

            app.UseAuthentication();
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

            await app.RunAsync();
        }
    }
}