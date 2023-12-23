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
    public class PaisController : ControllerBase
    {
        private readonly FicohsaContext _context;

        private readonly IHttpClientFactory _httpClientFactory;

        public PaisController(FicohsaContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/Pais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pais>>> GetPais()
        {
          if (_context.Pais == null)
          {
              return NotFound();
          }
            return await _context.Pais.ToListAsync();
        }

        // GET: api/Pais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pais>> GetPais(int id)
        {
          if (_context.Pais == null)
          {
              return NotFound();
          }
            var pais = await _context.Pais.FindAsync(id);

            if (pais == null)
            {
                return NotFound();
            }

            return pais;
        }

        // PUT: api/Pais/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPais(int id, Pais pais)
        {
            if (id != pais.IdPais)
            {
                return BadRequest();
            }

            _context.Entry(pais).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateCountries")
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
                if (!PaisExists(id))
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

        // POST: api/Pais
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pais>> PostPais(Pais pais)
        {
          if (_context.Pais == null)
          {
              return Problem("Entity set 'FicohsaContext.Pais'  is null.");
          }
            _context.Pais.Add(pais);
            await _context.SaveChangesAsync();
            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateCountries")
            {
                Headers =
            {
                { HeaderNames.Accept, "application/json" },
                },

            };
            var httpClient = _httpClientFactory.CreateClient();
            await httpClient.SendAsync(httpRequestMessage);
            return CreatedAtAction("GetPais", new { id = pais.IdPais }, pais);
        }

        // DELETE: api/Pais/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePais(int id)
        {
            if (_context.Pais == null)
            {
                return NotFound();
            }
            var pais = await _context.Pais.FindAsync(id);
            if (pais == null)
            {
                return NotFound();
            }

            _context.Pais.Remove(pais);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaisExists(int id)
        {
            return (_context.Pais?.Any(e => e.IdPais == id)).GetValueOrDefault();
        }
    }
}
