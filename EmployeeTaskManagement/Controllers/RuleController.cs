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
    public class RuleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RuleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rule
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rules.ToListAsync());
        }

        // GET: Rule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rule = await _context.Rules
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rule == null)
            {
                return NotFound();
            }

            return View(rule);
        }

        // GET: Rule/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Rule rule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rule);
        }

        // GET: Rule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rule = await _context.Rules.FindAsync(id);
            if (rule == null)
            {
                return NotFound();
            }
            return View(rule);
        }

        // POST: Rule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Rule rule)
        {
            if (id != rule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RuleExists(rule.Id))
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
            return View(rule);
        }

    

  
        public async Task<IActionResult> Delete(int id)
        {
            var rule = await _context.Rules.FindAsync(id);
            if (rule != null)
            {
                _context.Rules.Remove(rule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RuleExists(int id)
        {
            return _context.Rules.Any(e => e.Id == id);
        }
    }
}
