using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeTaskManagement.Data;
using EmployeeTaskManagement.Models;

namespace EmployeeTaskManagement.Controllers
{
    public class ShiftController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShiftController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Shift
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shifts.ToListAsync());
        }

        // GET: Shift/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shifts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // GET: Shift/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shift/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Shift shift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shift);
        }

        // GET: Shift/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }
            return View(shift);
        }

        // POST: Shift/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Shift shift)
        {
            if (id != shift.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShiftExists(shift.Id))
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
            return View(shift);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            if (shift != null)
            {
                _context.Shifts.Remove(shift);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShiftExists(int id)
        {
            return _context.Shifts.Any(e => e.Id == id);
        }
    }
}
