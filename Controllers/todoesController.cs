using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class todoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public todoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: todoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.todo.ToListAsync());
        }

        // GET: todoes show by search
        public async Task<IActionResult> ShowSearch()
        {
            return View();
        }

        // POST: todoes Search result
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.todo.Where( j => j.status.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: todoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.todo
                .FirstOrDefaultAsync(m => m.id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        [Authorize]
        // GET: todoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: todoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,todolist,status")] todo todo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: todoes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.todo.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return View(todo);
        }

        // POST: todoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,todolist,status")] todo todo)
        {
            if (id != todo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!todoExists(todo.id))
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
            return View(todo);
        }

        // GET: todoes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.todo
                .FirstOrDefaultAsync(m => m.id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: todoes/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todo = await _context.todo.FindAsync(id);
            if (todo != null)
            {
                _context.todo.Remove(todo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool todoExists(int id)
        {
            return _context.todo.Any(e => e.id == id);
        }
    }
}
