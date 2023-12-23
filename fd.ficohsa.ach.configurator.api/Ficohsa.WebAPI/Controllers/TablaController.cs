using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ficohsa.Models;
using Ficohsa.DAO;
using Ficohsa.Models.Auditable;

namespace Ficohsa.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablaController : ControllerBase
    {
        private readonly FicohsaContext _context;

        public TablaController(FicohsaContext context)
        {
            _context = context;
        }

        // GET: api/Tabla
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tabla>>> GetTabla()
        {
          if (_context.Tabla == null)
          {
              return NotFound();
          }
            return await _context.Tabla.ToListAsync();
        }

        // GET: api/Tabla/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tabla>> GetTabla(int id)
        {
          if (_context.Tabla == null)
          {
              return NotFound();
          }
            var tabla = await _context.Tabla.FindAsync(id);

            if (tabla == null)
            {
                return NotFound();
            }

            return tabla;
        }

        // PUT: api/Tabla/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTabla(int id, Tabla tabla)
        {
            if (id != tabla.IdTabla)
            {
                return BadRequest();
            }

            tabla.UpdateSave();
            _context.Entry(tabla).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TablaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tabla
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tabla>> PostTabla(Tabla tabla)
        {
          if (_context.Tabla == null)
          {
              return Problem("Entity set 'FicohsaContext.Tabla'  is null.");
          }
            tabla.CreateSave();
            _context.Tabla.Add(tabla);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTabla", new { id = tabla.IdTabla }, tabla);
        }

        // DELETE: api/Tabla/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTabla(int id)
        {
            if (_context.Tabla == null)
            {
                return NotFound();
            }
            var tabla = await _context.Tabla.FindAsync(id);
            if (tabla == null)
            {
                return NotFound();
            }

            _context.Tabla.Remove(tabla);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TablaExists(int id)
        {
            return (_context.Tabla?.Any(e => e.IdTabla == id)).GetValueOrDefault();
        }
    }
}
