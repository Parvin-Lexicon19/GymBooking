using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymBooking.Data;
using GymBooking.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GymBooking.Controllers
{
    [Authorize]
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GymClassesController(ApplicationDbContext context, UserManager <ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: GymClasses
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //if(!User.Identity.IsAuthenticated)
            //{
            //    return View(await _context.GymClasses.ToListAsync());
            //}
            var model = await _context.GymClasses
                .Include(g => g.AttendingMembers)
                .ThenInclude(a => a.ApplicationUser)
                .ToListAsync();
            return View(model);
        }

        // GET: GymClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var gymClass = await _context.GymClasses
                .Include(c => c.AttendingMembers)
                .ThenInclude(e => e.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            //var gymClass = await _context.GymClasses
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // GET: GymClasses/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gymClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymClass = await _context.GymClasses.FindAsync(id);
            _context.GymClasses.Remove(gymClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymClassExists(int id)
        {
            return _context.GymClasses.Any(e => e.Id == id);
        }

        public async Task<IActionResult> BookingToggle(int? id)
        {
            if (id == null)
                return NotFound();
            //var userId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var userId = _userManager.GetUserId(User);

            var currentGymClass = await _context.GymClasses
                .Include(a => a.AttendingMembers)
               .FirstOrDefaultAsync(g => g.Id == id);

             var attending = currentGymClass.AttendingMembers
                .FirstOrDefault(u => u.ApplicationUserId == userId);
            if (attending == null)
            {
                var book = new ApplicationUserGymClass
                {
                    ApplicationUserId = userId,
                    GymClassId = currentGymClass.Id
                };
                _context.ApplicationUserGymClasses.Add(book);
                _context.SaveChanges();
            }

            else
            {
                _context.ApplicationUserGymClasses.Remove(attending);
                _context.SaveChanges();
            }
            //var pass = await _context.GymClasses
            //   .Include(c => c.AttendingMembers)
            //   .ThenInclude(e => e.ApplicationUser)
            //   .FirstOrDefaultAsync(m => m.Id == id);
           
            //if (pass != null)
            //{
            //    _context.GymClasses.Remove(pass);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //if(pass == null)
            //{
            //    _context.GymClasses.Add(pass);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}



            //var gymClass = await _context.GymClasses
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (gymClass == null)
            //{
            //    return NotFound();
            //}

           return RedirectToAction(nameof(Index));
        }
    }
}
