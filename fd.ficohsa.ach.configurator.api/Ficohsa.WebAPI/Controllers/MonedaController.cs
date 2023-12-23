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
    public class MonedaController : ControllerBase
    {
        private readonly FicohsaContext _context;

        private readonly IHttpClientFactory _httpClientFactory;

        public MonedaController(FicohsaContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/Moneda
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moneda>>> GetMonedaList(int idPais)
        {
            if (_context.Moneda == null)
            {
                return NotFound();
            }
            return await _context.Moneda.Where(b => b.IdPais == idPais).ToListAsync();
        }

        // GET: api/Moneda/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Moneda>> GetMoneda(int id)
        {
          if (_context.Moneda == null)
          {
              return NotFound();
          }
            var moneda = await _context.Moneda.FindAsync(id);

            if (moneda == null)
            {
                return NotFound();
            }

            return moneda;
        }

        // PUT: api/Moneda/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMoneda(int id, Moneda moneda)
        {
            if (id != moneda.IdMoneda)
            {
                return BadRequest();
            }

            moneda.UpdateSave();
            _context.Entry(moneda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var country = await _context.Pais.FirstAsync(p => p.IdPais == moneda.IdPais);
                var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateCurrencies?countryCode=" + country.CodigoISO)
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
                if (!MonedaExists(id))
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

        // POST: api/Moneda
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Moneda>> PostMoneda(Moneda moneda)
        {
          if (_context.Moneda == null)
          {
              return Problem("Entity set 'FicohsaContext.Moneda'  is null.");
            }
            moneda.CreateSave();
            _context.Moneda.Add(moneda);
            await _context.SaveChangesAsync();

            var country = await _context.Pais.FirstAsync(p => p.IdPais == moneda.IdPais);
            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateCurrencies?countryCode=" + country.CodigoISO)
            {
                Headers =
            {
                { HeaderNames.Accept, "application/json" },
                },

            };
            var httpClient = _httpClientFactory.CreateClient();
            await httpClient.SendAsync(httpRequestMessage);
            return CreatedAtAction("GetMoneda", new { id = moneda.IdMoneda }, moneda);
        }

        // DELETE: api/Moneda/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoneda(int id)
        {
            if (_context.Moneda == null)
            {
                return NotFound();
            }
            var moneda = await _context.Moneda.FindAsync(id);
            if (moneda == null)
            {
                return NotFound();
            }

            _context.Moneda.Remove(moneda);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MonedaExists(int id)
        {
            return (_context.Moneda?.Any(e => e.IdMoneda == id)).GetValueOrDefault();
        }
    }
}
