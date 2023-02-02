using Ella.BLL.Helpers;
using Ella.Core.Entity;
using Ella.DAL.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ella.DAL.DAL
{
    public class DataIntializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _Dbcontext;
        private readonly AdminUser _adminUser;
        public DataIntializer(IServiceProvider serviceProvider)
        {
            _adminUser = serviceProvider.GetRequiredService<IOptions<AdminUser>>().Value;
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
             _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
             _Dbcontext = serviceProvider.GetRequiredService<AppDbContext>();
        }
       public async Task SeedData()
        {
            await _Dbcontext.Database.MigrateAsync();
            
            var roles = new List<string> { Constants.AdminRole, Constants.UserRole };

            foreach (var role in roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                    continue;
                var result = await _roleManager.CreateAsync(new IdentityRole { Name = role });

                if (!result.Succeeded)
                    foreach (var error in result.Errors)
                    {
                        //logging
                        Console.WriteLine(error.Description);
                    }
                var userExist = await _userManager.FindByNameAsync(_adminUser.UserName);

                if (userExist != null)
                    return;

                var userResult = await _userManager.CreateAsync(new User
                {
                    UserName = _adminUser.UserName,
                    Email = _adminUser.Email
                },  _adminUser.Password);
                if(!userResult.Succeeded)
                {
                    foreach (var error in userResult.Errors)
                    {
                        //logging
                        Console.WriteLine(error.Description);
                    }
                }
                else
                {
                    var exsitUser = await _userManager.FindByNameAsync(_adminUser.UserName);
                    await _userManager.AddToRoleAsync(exsitUser, Constants.AdminRole);
                }
                 
            }
        }
            
    }
}
