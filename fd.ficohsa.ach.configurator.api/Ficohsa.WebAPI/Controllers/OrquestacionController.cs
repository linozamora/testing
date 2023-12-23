using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ficohsa.Models;
using Ficohsa.DAO;
using Microsoft.Net.Http.Headers;
using System.Net.Http;

namespace Ficohsa.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrquestacionController : ControllerBase
    {
        private readonly FicohsaContext _context;

        private readonly IHttpClientFactory _httpClientFactory;

        public OrquestacionController(FicohsaContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/Orquestacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orquestacion>>> GetOrquestacion()
        {
          if (_context.Orquestacion == null)
          {
              return NotFound();
          }
            return await _context.Orquestacion.ToListAsync();
        }

        // GET: api/Orquestacion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orquestacion>> GetOrquestacion(int id)
        {
          if (_context.Orquestacion == null)
          {
              return NotFound();
          }
            var orquestacion = await _context.Orquestacion.FindAsync(id);

            if (orquestacion == null)
            {
                return NotFound();
            }

            return orquestacion;
        }

        // PUT: api/Orquestacion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrquestacion(int id, Orquestacion orquestacion)
        {
            if (id != orquestacion.IdOrquestacion)
            {
                return BadRequest();
            }

            _context.Entry(orquestacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var country = await _context.Pais.FirstAsync(p => p.IdPais == orquestacion.IdPais);
                var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateOrchestration?countryCode=" + country.CodigoISO)
                {
                    Headers =
            {
                { HeaderNames.Accept, "application/json" },
                    },

                };
                var httpClient = _httpClientFactory.CreateClient();
                await httpClient.SendAsync(httpRequestMessage);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrquestacionExists(id))
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

        // POST: api/Orquestacion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Orquestacion>> PostOrquestacion(Orquestacion orquestacion)
        {
          if (_context.Orquestacion == null)
          {
              return Problem("Entity set 'FicohsaContext.Orquestacion'  is null.");
          }
            _context.Orquestacion.Add(orquestacion);
            await _context.SaveChangesAsync();

            var country = await _context.Pais.FirstAsync(p => p.IdPais == orquestacion.IdPais);
            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateOrchestration?countryCode=" + country.CodigoISO)
            {
                Headers =
            {
                { HeaderNames.Accept, "application/json" },
                },

            };
            var httpClient = _httpClientFactory.CreateClient();
            await httpClient.SendAsync(httpRequestMessage);
            return CreatedAtAction("GetOrquestacion", new { id = orquestacion.IdOrquestacion }, orquestacion);
        }

        // DELETE: api/Orquestacion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrquestacion(int id)
        {
            if (_context.Orquestacion == null)
            {
                return NotFound();
            }
            var orquestacion = await _context.Orquestacion.FindAsync(id);
            if (orquestacion == null)
            {
                return NotFound();
            }

            _context.Orquestacion.Remove(orquestacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrquestacionExists(int id)
        {
            return (_context.Orquestacion?.Any(e => e.IdOrquestacion == id)).GetValueOrDefault();
        }
    }
}
