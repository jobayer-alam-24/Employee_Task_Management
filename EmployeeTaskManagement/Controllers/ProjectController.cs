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
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EmployeeTaskManagement.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationMember> _userManager;

        public ProjectController(ApplicationDbContext context, UserManager<ApplicationMember> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Project
        public async Task<IActionResult> Index()
        {
            
            var projects = await _context.Projects
                .Include(x => x.ProjectMembers).ThenInclude(x => x.Member).ToListAsync();
            return View(projects);
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(x=>x.ProjectMembers).ThenInclude(x=>x.Member)
                .Include(x=>x.Items).ThenInclude(x=>x.Members).ThenInclude(x=>x.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            ViewBag.Shifts = new SelectList(await _context.Shifts.ToListAsync(), "Id", "Name");
            return View(project);
        }

        // GET: Project/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new ProjectCreateViewModel();
            var members = await _userManager.Users.ToListAsync();
            model.Members = members.Select(member => new MemberViewModel()
            {
                MemberId = member.Id,
                MemberName = member.Name ?? "Unknown",
                IsSelected = false,

            }).ToList();
            return View(model);
        }

        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.                           
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProjectCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(model.Project);
                await _context.SaveChangesAsync();


                if (model.Members.Any())
                {
                    for (int i = 0; i < model.Members.Count; i++)
                    {
                        if (model.Members[i].IsSelected)
                        {
                            model.Project.ProjectMembers.Add(new ProjectMember
                            {
                                MemberId = model.Members[i].MemberId
                            });


                        }
                    }
                    await _context.SaveChangesAsync();
                }


                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Project/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                                        .Include(p => p.ProjectMembers)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            var members = await _context.Users.ToListAsync();  

            var viewModel = new ProjectCreateViewModel
            {
                Project = project,
                Members = members.Select(u => new MemberViewModel
                {
                    MemberId = u.Id,
                    MemberName = u.Name,
                    IsSelected = project.ProjectMembers.Any(pm => pm.MemberId == u.Id) 
                }).ToList()
            };

            return View(viewModel);
        }



        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, ProjectCreateViewModel viewModel)
        {
            if (id != viewModel.Project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var project = await _context.Projects
                                                .Include(p => p.ProjectMembers)
                                                .FirstOrDefaultAsync(p => p.Id == id);

                    if (project == null)
                    {
                        return NotFound();
                    }

                    project.Name = viewModel.Project.Name;
                    project.Title = viewModel.Project.Title;

                    var selectedMemberIds = viewModel.Members.Where(m => m.IsSelected).Select(m => m.MemberId).ToList();

                    var projectMembersToRemove = project.ProjectMembers
                                                         .Where(pm => !selectedMemberIds.Contains(pm.MemberId))
                                                         .ToList();
                    foreach (var pm in projectMembersToRemove)
                    {
                        project.ProjectMembers.Remove(pm);
                    }

                    var existingMemberIds = project.ProjectMembers.Select(pm => pm.MemberId).ToList();
                    foreach (var memberId in selectedMemberIds)
                    {
                        if (!existingMemberIds.Contains(memberId))
                        {
                            project.ProjectMembers.Add(new ProjectMember
                            {
                                ProjectId = project.Id,
                                MemberId = memberId
                            });
                        }
                    }

                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(viewModel.Project.Id))
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

            return View(viewModel);
        }




        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
       
        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
