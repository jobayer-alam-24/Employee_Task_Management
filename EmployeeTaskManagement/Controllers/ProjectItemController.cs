using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeTaskManagement.Data;
using EmployeeTaskManagement.Models;
using EmployeeTaskManagement.ViewModels;
using NuGet.Versioning;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.Controllers
{
    public class ProjectItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectItem
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProjectItems.Include(p => p.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectItem = await _context.ProjectItems
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectItem == null)
            {
                return NotFound();
            }

            return View(projectItem);
        }

        // GET: ProjectItem/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            return View();
        }

        // POST: ProjectItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _context.ProjectItems.AddAsync(model.Item);
                await _context.SaveChangesAsync();
                if (model.Members.Any())
                {
                    for (int i = 0; i < model.Members.Count; i++)
                    {
                        if (model.Members[i].IsSelected)
                        {


                            model.Item.Members.Add(new ProjectItemMember
                            {
                                MemberId = model.Members[i].MemberId,
                                Status = ProjectItemAssignStatus.Assigned
                            });


                        }
                    }
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Details", "Project", new { id = model.Item.ProjectId });
            }

            return RedirectToAction("Details", "Project", new { id = model.Item.ProjectId });

        }

        // GET: ProjectItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectItem = await _context.ProjectItems.FindAsync(id);
            if (projectItem == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", projectItem.ProjectId);
            return View(projectItem);
        }

        // POST: ProjectItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,AssignDate,WorkingHours,ProjectId")] ProjectItem projectItem)
        {
            if (id != projectItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectItemExists(projectItem.Id))
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
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", projectItem.ProjectId);
            return View(projectItem);
        }

      

        public async Task<IActionResult> Delete(int id)
        {
            var projectItem = await _context.ProjectItems.FindAsync(id);
            if (projectItem != null)
            {
                _context.ProjectItems.Remove(projectItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> AssignItemUpdate(int id, int projectId)
        {
             if(id<=0) return NotFound("Id is not given");
             var task = await _context.ProjectItemAssigns.Include(x=>x.ProjectItem).FirstOrDefaultAsync(x=>x.Id == id);
            if(task != null)
            {
                var model = new UpdateTaskViewModel
                {
                    Id = task.Id,
                    Name = task.ProjectItem.Name,
                    Description = task.Description,
                    Status = task.Status,

                };

                ViewBag.ProjectId = projectId;
                return View(model);


            }
            return NotFound();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignItemUpdate(int id, UpdateTaskViewModel model)
        {
            if (id <= 0) return NotFound("Id is not given");
            
            if (ModelState.IsValid)
            {
                var existingTask = await _context.ProjectItemAssigns.Include(x => x.ProjectItem).FirstOrDefaultAsync(x => x.Id == id);
                if (existingTask != null)
                {
                    existingTask.Description = model.Description;
                    existingTask.Status = model.Status;

                    _context.ProjectItemAssigns.Update(existingTask);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Project", new { id = model.ProjectId });
                }
            }
            return View(model);

        }

        private bool ProjectItemExists(int id)
        {
            return _context.ProjectItems.Any(e => e.Id == id);
        }
    }
}
