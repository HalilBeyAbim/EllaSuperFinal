using Ella.Core.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ella.DAL.DAL
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<WishListProduct> WishListProduct { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Banner> Banners { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>().HasIndex(x => x.Key).IsUnique();
            base.OnModelCreating(modelBuilder);  
        }
        
    }
}
