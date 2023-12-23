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
    public class CanalController : ControllerBase
    {
        private readonly FicohsaContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public CanalController(FicohsaContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/Canal
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Canal>>> GetCanalList(int idPais)
        {
            if (_context.Canal == null)
            {
                return NotFound();
            }
            return await _context.Canal.Where(b => b.IdPais == idPais).ToListAsync();
        }

        // GET: api/Canal/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Canal>> GetCanal(int id)
        {
          if (_context.Canal == null)
          {
              return NotFound();
          }
            var canal = await _context.Canal.FindAsync(id);

            if (canal == null)
            {
                return NotFound();
            }

            return canal;
        }

        // PUT: api/Canal/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCanal(int id, Canal canal)
        {
            if (id != canal.IdCanal)
            {
                return BadRequest();
            }

            canal.UpdateSave();
            _context.Entry(canal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var country = await _context.Pais.FirstAsync(p => p.IdPais == canal.IdPais);
                var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateChannels?countryCode=" + country.CodigoISO)
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
                if (!CanalExists(id))
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

        // POST: api/Canal
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Canal>> PostCanal(Canal canal)
        {
          if (_context.Canal == null)
          {
              return Problem("Entity set 'FicohsaContext.Canal'  is null.");
            }
            canal.CreateSave();
            _context.Canal.Add(canal);
            await _context.SaveChangesAsync();

            var country = await _context.Pais.FirstAsync(p => p.IdPais == canal.IdPais);
            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateChannels?countryCode=" + country.CodigoISO)
            {
                Headers =
            {
                { HeaderNames.Accept, "application/json" },
                },

            };
            var httpClient = _httpClientFactory.CreateClient();
            await httpClient.SendAsync(httpRequestMessage);
            return CreatedAtAction("GetCanal", new { id = canal.IdCanal }, canal);
        }

        // DELETE: api/Canal/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCanal(int id)
        {
            if (_context.Canal == null)
            {
                return NotFound();
            }
            var canal = await _context.Canal.FindAsync(id);
            if (canal == null)
            {
                return NotFound();
            }

            _context.Canal.Remove(canal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CanalExists(int id)
        {
            return (_context.Canal?.Any(e => e.IdCanal == id)).GetValueOrDefault();
        }
    }
}
