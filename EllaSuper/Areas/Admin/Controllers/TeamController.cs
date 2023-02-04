using Ella.BLL.Extensions;
using Ella.BLL.Helpers;
using Ella.Core.Entity;
using Ella.DAL.DAL;
using EllaSuper.Areas.Admin.Models;
using EllaSuperFinal.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.Areas.Admin.Controllers
{
    public class TeamController : BaseController
    {
        private readonly AppDbContext _AppDbContext;
        public TeamController(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var team = await _AppDbContext.Teams.ToListAsync();
            return View(team);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var existTeam = await _AppDbContext.Teams.Where(c => c.IsDeleted).ToListAsync();
            if (existTeam.Any(c => c.Name.ToLower().Trim().Equals(model.Name.ToLower().Trim())))
            {
                ModelState.AddModelError("Name", "This Team is already exist");
                return View();
            }
            if (!ModelState.IsValid)
                return View(model);
            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "You must choose photo");
                return View();
            }
            if (!model.Image.IsAllowedSize(5))
            {
                ModelState.AddModelError("Image", "Image size is over 5MB, Please select less than 5 mb");
                return View();
            }
            var unicalName = await model.Image.GenerateFile(Constants.TeamPath);
            var team = new Team
            {
                Name = model.Name,
                Position = model.Position,
                ImageUrl = unicalName,
            };
            await _AppDbContext.Teams.AddAsync(team);
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var team = await _AppDbContext.Teams.FirstOrDefaultAsync(c => c.Id == id);
            if (team == null) return NotFound();
            var model = new TeamUpdateViewModel
            {
                Name = team.Name,
                Position = team.Position,
                ImageUrl = team.ImageUrl,
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, TeamUpdateViewModel model)
        {
            if (id == null) return NotFound();
            var team = await _AppDbContext.Teams.FirstOrDefaultAsync(c => c.Id == id);
            if (team == null) return NotFound();
            if (!ModelState.IsValid) return View(model);
            var existTeam = await _AppDbContext.Teams.Where(c => c.IsDeleted).ToListAsync();
            if (existTeam.Any(c => c.Name.ToLower().Trim().Equals(model.Name.ToLower().Trim())))
            {
                ModelState.AddModelError("Name", "This Team is already exist");
                return View();
            }
            if (!ModelState.IsValid)
                return View(model);
            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "You must choose photo");
                return View();
            }
            if (!model.Image.IsAllowedSize(5))
            {
                ModelState.AddModelError("Image", "Image size is over 5MB, Please select less than 5 mb");
                return View();
            }
            if (model.Image != null)
            {
                var unicalName = await model.Image.GenerateFile(Constants.TeamPath);
                team.ImageUrl = unicalName;

            }
            
            team.Name = model.Name;
            team.Position = model.Position;
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var team = await _AppDbContext.Teams.FirstOrDefaultAsync(c => c.Id == id);
            if (team == null) return NotFound();
            _AppDbContext.Teams.Remove(team);
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
    
}
