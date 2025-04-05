using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeTaskManagement.Data;
using EmployeeTaskManagement.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using EmployeeTaskManagement.ViewModels;

namespace EmployeeTaskManagement.Controllers
{
    public class RuleViolationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RuleViolationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RuleViolation
        public async Task<IActionResult> Index()
        {
            var ruleViolations = _context.MemberRuleViolations.Include(r => r.Member).Include(r => r.Rule).Where(x=>x.MemberId == GetUserId());
            if (IsInRole("Admin"))
            {
                ruleViolations = _context.MemberRuleViolations.Include(x=>x.Member).Include(x=>x.Rule);
            }
            return View(await ruleViolations.ToListAsync());
        }

        // GET: RuleViolation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruleViolation = await _context.MemberRuleViolations
                .Include(r => r.Member)
                .Include(r => r.Rule)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ruleViolation == null)
            {
                return NotFound();
            }

            return View(ruleViolation);
        }

        public async Task<IActionResult> UpdateByMember(int id)
        {
            if (id <= 0) return NotFound("Id is not found.");
            var ruleViolation = await _context.MemberRuleViolations
                .Include(r => r.Member)
                .Include(r => r.Rule)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ruleViolation == null)
            {
                return NotFound();
            }

            var model = new UpdateViolationViewModel
            {
                Id = ruleViolation.Id,
                Feedback = ruleViolation.MemberNote
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateByMember(int id, UpdateViolationViewModel model)
        {

            if (id <= 0) return NotFound("Id is not found.");
            if (ModelState.IsValid)
            {
                var existingViolation = await _context.MemberRuleViolations
                    .Include(r => r.Member)
                    .Include(r => r.Rule)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (existingViolation == null)
                {
                    return NotFound();
                }

                existingViolation.MemberNote = model.Feedback;
                existingViolation.ReplyStatus =  RuleViolationReplyStatus.Replied; 

                _context.MemberRuleViolations.Update(existingViolation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(model);
        }

        // GET: RuleViolation/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Name");
            ViewData["RuleId"] = new SelectList(_context.Rules, "Id", "Name");
            return View();
        }

        // POST: RuleViolation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,MemberId,IncidentDate,RuleId,Point,Note,MemberNote,ReplyStatus,Status")] RuleViolation ruleViolation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ruleViolation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Name", ruleViolation.MemberId);
            ViewData["RuleId"] = new SelectList(_context.Rules, "Id", "Name", ruleViolation.RuleId);
            return View(ruleViolation);
        }

        // GET: RuleViolation/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruleViolation = await _context.MemberRuleViolations.FindAsync(id);
            if (ruleViolation == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Name", ruleViolation.MemberId);
            ViewData["RuleId"] = new SelectList(_context.Rules, "Id", "Name", ruleViolation.RuleId);
            return View(ruleViolation);
        }

        // POST: RuleViolation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MemberId,IncidentDate,RuleId,Point,Note,MemberNote,ReplyStatus,Status")] RuleViolation ruleViolation)
        {
            if (id != ruleViolation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ruleViolation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RuleViolationExists(ruleViolation.Id))
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
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Name", ruleViolation.MemberId);
            ViewData["RuleId"] = new SelectList(_context.Rules, "Id", "Name", ruleViolation.RuleId);
            return View(ruleViolation);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var ruleViolation = await _context.MemberRuleViolations.FindAsync(id);
            if (ruleViolation != null)
            {
                _context.MemberRuleViolations.Remove(ruleViolation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RuleViolationExists(int id)
        {
            return _context.MemberRuleViolations.Any(e => e.Id == id);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }


        private bool IsInRole(string rolename)
        {
            return User.IsInRole(rolename);
        }
    }
}
