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
using Microsoft.Net.Http.Headers;
using System.Net.Http;

namespace Ficohsa.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoController : ControllerBase
    {
        private readonly FicohsaContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public CatalogoController(FicohsaContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/Catalogo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Catalogo>>> GetCatalogoList(int idTabla)
        {
            if (_context.Catalogo == null)
            {
                return NotFound();
            }
            return await _context.Catalogo.Where(b => b.IdTabla == idTabla).ToListAsync();
        }

        // GET: api/Catalogo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Catalogo>> GetCatalogo(int id)
        {
          if (_context.Catalogo == null)
          {
              return NotFound();
          }
            var catalogo = await _context.Catalogo.FindAsync(id);

            if (catalogo == null)
            {
                return NotFound();
            }

            return catalogo;
        }

        // PUT: api/Catalogo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCatalogo(int id, Catalogo catalogo)
        {
            if (id != catalogo.IdCatalogo)
            {
                return BadRequest();
            }

            catalogo.UpdateSave();
            _context.Entry(catalogo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                var country = await _context.Pais.FirstAsync(p => p.IdPais == catalogo.IdPais);
                var tabla = await _context.Tabla.FirstAsync(t => t.IdTabla == catalogo.IdTabla);
                var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateCatalogs?countryCode=" + country.CodigoISO + "&table=" + tabla.Nombre)
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
                if (!CatalogoExists(id))
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

        // POST: api/Catalogo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Catalogo>> PostCatalogo(Catalogo catalogo)
        {
          if (_context.Catalogo == null)
          {
              return Problem("Entity set 'FicohsaContext.Catalogo'  is null.");
            }
            catalogo.CreateSave();
            _context.Catalogo.Add(catalogo);
            await _context.SaveChangesAsync();

            var country = await _context.Pais.FirstAsync(p => p.IdPais == catalogo.IdPais);
            var tabla = await _context.Tabla.FirstAsync(t => t.IdTabla == catalogo.IdTabla);
            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateCatalogs?countryCode=" + country.CodigoISO + "&table=" + tabla.Nombre)
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                },
            };
            var httpClient = _httpClientFactory.CreateClient();
            await httpClient.SendAsync(httpRequestMessage);
            return CreatedAtAction("GetCatalogo", new { id = catalogo.IdCatalogo }, catalogo);
        }

        // DELETE: api/Catalogo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCatalogo(int id)
        {
            if (_context.Catalogo == null)
            {
                return NotFound();
            }
            var catalogo = await _context.Catalogo.FindAsync(id);
            if (catalogo == null)
            {
                return NotFound();
            }

            _context.Catalogo.Remove(catalogo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CatalogoExists(int id)
        {
            return (_context.Catalogo?.Any(e => e.IdCatalogo == id)).GetValueOrDefault();
        }
    }
}
