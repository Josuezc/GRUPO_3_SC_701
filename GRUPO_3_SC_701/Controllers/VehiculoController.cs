using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GRUPO_3_SC_701.Data;
using GRUPO_3_SC_701.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GRUPO_3_SC_701.Controllers
{
   [Authorize(Roles ="Admin")]
    public class VehiculoController : Controller
    {
        private readonly ApplicationDbContext _context;

       
        public VehiculoController(ApplicationDbContext context)
        {
            _context = context;
           
        }

        // GET: Vehiculo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vehiculos.Include(v => v.UsuarioRegistro);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vehiculo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos
                .Include(v => v.UsuarioRegistro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

  


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roleName = "Conductor";
            var roleId = await _context.Roles
                .Where(r => r.Name == roleName)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            var usuariosConRol = await _context.Users
                .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == roleId)).ToListAsync();
            ViewData["UsuarioRegistroId"] = new SelectList(usuariosConRol, "Id", "UserName");
            return View();
        }

      

        [HttpPost]
        public async Task<IActionResult> Create(Vehiculo vehiculo)
        {          
          
            vehiculo.FechaRegistro = DateTime.Now;
           

            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();


            var roleName = "Conductor";
            var roleId = await _context.Roles.Where(r => r.Name == roleName).Select(r => r.Id).FirstOrDefaultAsync();
            var usuariosConRol = await _context.Users.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == roleId)).ToListAsync();
            ViewData["UsuarioRegistroId"] = new SelectList(usuariosConRol, "Id", "UserName", vehiculo.UsuarioRegistroId);
            return RedirectToAction("Index");
        }
        // GET: Vehiculo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }
            var roleName = "Conductor";
            var roleId = await _context.Roles.Where(r => r.Name == roleName).Select(r => r.Id).FirstOrDefaultAsync();
            var usuariosConRol = await _context.Users.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == roleId)).ToListAsync();
            ViewData["UsuarioRegistroId"] = new SelectList(usuariosConRol, "Id", "UserName", vehiculo.UsuarioRegistroId);
            return View(vehiculo);
        }

        // POST: Vehiculo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Placa,Modelo,Capacidad,Estado,FechaRegistro,UsuarioRegistroId")] Vehiculo vehiculo)
        {
            if (id != vehiculo.Id)
            {
                return NotFound();
            }
            //vehiculo.FechaRegistro = DateTime.Now;
            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehiculoExists(vehiculo.Id))
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
          
            var roleName = "Conductor";
            var roleId = await _context.Roles.Where(r => r.Name == roleName).Select(r => r.Id).FirstOrDefaultAsync();
            var usuariosConRol = await _context.Users.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == roleId)).ToListAsync();
            ViewData["UsuarioRegistroId"] = new SelectList(usuariosConRol, "Id", "UserName", vehiculo.UsuarioRegistroId);
            return View(vehiculo);
        }

        // GET: Vehiculo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos
                .Include(v => v.UsuarioRegistro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // POST: Vehiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo != null)
            {
                _context.Vehiculos.Remove(vehiculo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehiculoExists(int id)
        {
            return _context.Vehiculos.Any(e => e.Id == id);
        }
    }
}
