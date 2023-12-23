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
using System.Text;

namespace Ficohsa.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametroController : ControllerBase
    {
        private readonly FicohsaContext _context;

        private readonly IHttpClientFactory _httpClientFactory;

        public ParametroController(FicohsaContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/Parametro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parametro>>> GetParametro()
        {
            if (_context.Parametro == null)
            {
                return NotFound();
            }
            var lstParameters = await _context.Parametro.ToListAsync();
            lstParameters.ForEach(p =>
            {
                p.Valor = p.CifrarValor ? Decrypt(p.Valor) : p.Valor;
            });
            return lstParameters;
        }

        // GET: api/Parametro/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Parametro>> GetParametro(int id)
        {
            if (_context.Parametro == null)
            {
                return NotFound();
            }
            var parametro = await _context.Parametro.FindAsync(id);

            if (parametro == null)
            {
                return NotFound();
            }

            parametro.Valor = parametro.CifrarValor ? Decrypt(parametro.Valor) : parametro.Valor;

            return parametro;
        }

        // PUT: api/Parametro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParametro(int id, Parametro parametro)
        {
            if (id != parametro.IdParametro)
            {
                return BadRequest();
            }

            parametro.Valor = parametro.CifrarValor ? Crypt(parametro.Valor) : parametro.Valor;

            parametro.UpdateSave();
            _context.Entry(parametro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var country = await _context.Pais.FirstAsync(p => p.IdPais == parametro.IdPais);
                var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateParameters?countryCode=" + country.CodigoISO)
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
                if (!ParametroExists(id))
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

        // POST: api/Parametro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Parametro>> PostParametro(Parametro parametro)
        {
            if (_context.Parametro == null)
            {
                return Problem("Entity set 'FicohsaContext.Parametro'  is null.");
            }
            parametro.Valor = parametro.CifrarValor ? Crypt(parametro.Valor) : parametro.Valor;
            parametro.CreateSave();
            _context.Parametro.Add(parametro);
            await _context.SaveChangesAsync();

            var country = await _context.Pais.FirstAsync(p => p.IdPais == parametro.IdPais);
            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateParameters?countryCode=" + country.CodigoISO)
            {
                Headers =
            {
                { HeaderNames.Accept, "application/json" },
                },

            };
            var httpClient = _httpClientFactory.CreateClient();
            await httpClient.SendAsync(httpRequestMessage);
            return CreatedAtAction("GetParametro", new { id = parametro.IdParametro }, parametro);
        }

        // DELETE: api/Parametro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParametro(int id)
        {
            if (_context.Parametro == null)
            {
                return NotFound();
            }
            var parametro = await _context.Parametro.FindAsync(id);
            if (parametro == null)
            {
                return NotFound();
            }

            _context.Parametro.Remove(parametro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParametroExists(int id)
        {
            return (_context.Parametro?.Any(e => e.IdParametro == id)).GetValueOrDefault();
        }

        private static string Crypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return string.Empty;
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var passwordbase64 = Convert.ToBase64String(plainTextBytes);
            var reversedString = new string(passwordbase64.Reverse().ToArray());
            return reversedString;
        }

        private static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return string.Empty;
            var encodedString = new string(cipherText.Reverse().ToArray());
            var data = Convert.FromBase64String(encodedString);
            var decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

    }
}
