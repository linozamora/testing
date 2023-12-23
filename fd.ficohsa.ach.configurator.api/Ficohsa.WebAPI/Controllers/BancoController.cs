using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ficohsa.Models;
using Ficohsa.DAO;
using Ficohsa.Models.Auditable;
using Microsoft.Net.Http.Headers;
using ServiceReference;
using System.ServiceModel;
using System.ServiceModel.Security;
//using System.Web.Http;

namespace Ficohsa.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BancoController : ControllerBase
    {
        private readonly FicohsaContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public BancoController(FicohsaContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // GET: api/Banco
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Banco>>> GetBancoList(int idPais)
        {
            return await _context.Banco.Where(b => b.IdPais == idPais).ToListAsync();
        }

        // GET: api/Banco/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Banco>> GetBanco(int id)
        {
            var banco = await _context.Banco.FindAsync(id);

            if (banco == null)
            {
                return NotFound();
            }

            return banco;
        }

        // PUT: api/Banco/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBanco(int id, Banco banco)
        {
            if (id != banco.IdBanco)
            {
                return BadRequest();
            }

            string sourceBank;
            string destinationBank;
            string username;
            string password;

            try
            {
                var parameters = _context.Parametro.ToList();
                sourceBank = parameters.Single(p => p.Nombre == _configuration["Sourcebank"]).Valor;
                destinationBank = parameters.Single(p => p.Nombre == _configuration["DestinationBank"]).Valor;
                username = parameters.Single(p => p.Nombre == _configuration["UsernameOSB"]).Valor;
                password = parameters.Single(p => p.Nombre == _configuration["PasswordOSB"]).Valor;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            await using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //wiping out the redis key
                var country = await _context.Pais.FirstAsync(p => p.IdPais == banco.IdPais);
                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Post,
                    "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateBanks?countryCode=" + country.CodigoISO)
                {
                    Headers =
                    {
                        { HeaderNames.Accept, "application/json" },
                    },

                };
                var httpClient = _httpClientFactory.CreateClient();
                await httpClient.SendAsync(httpRequestMessage);
                //saving the db record
                banco.UpdateSave();
                _context.Entry(banco).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                //updating the core banking record type "Bank"
                _ = await UpdateCorebanking(banco, "Bank", username, password, sourceBank, destinationBank);
                //saving the db records
                await dbTransaction.CommitAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest($"Error al insertar registro {e.InnerException?.Message ?? string.Empty}");
            }
            catch (Exception e)
            {
                await dbTransaction.RollbackAsync();
                return BadRequest(e.Message);
            }
            return NoContent();
        }

        // POST: api/Banco
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Banco>> PostBanco(Banco banco)
        {
            string sourceBank;
            string destinationBank;
            string username;
            string password;

            try
            {
                var parameters = _context.Parametro.ToList();
                sourceBank = parameters.Single(p => p.Nombre == _configuration["Sourcebank"]).Valor;
                destinationBank = parameters.Single(p => p.Nombre == _configuration["DestinationBank"]).Valor;
                username = parameters.Single(p => p.Nombre == _configuration["UsernameOSB"]).Valor;
                password = parameters.Single(p => p.Nombre == _configuration["PasswordOSB"]).Valor;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            await using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //wiping out the redis key
                var country = await _context.Pais.FirstAsync(p => p.IdPais == banco.IdPais);
                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Post,
                    "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateBanks?countryCode=" + country.CodigoISO)
                {
                    Headers =
                    {
                        { HeaderNames.Accept, "application/json" },
                    },

                };
                var httpClient = _httpClientFactory.CreateClient();
                await httpClient.SendAsync(httpRequestMessage);

                //saving the db record
                banco.CreateSave();
                _context.Banco.Add(banco);
                await _context.SaveChangesAsync();
                //updating the core banking record type "Bank"
                _ = await UpdateCorebanking(banco, "Bank", username, password, sourceBank, destinationBank);
                //saving the db records
                await dbTransaction.CommitAsync();
                return CreatedAtAction("GetBanco", new { id = banco.IdBanco }, banco);
            }
            catch (DbUpdateException e)
            {
                return BadRequest($"Error al insertar registro {e.InnerException?.Message ?? string.Empty}");
            }
            catch (Exception e)
            {
                await dbTransaction.RollbackAsync();
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Banco/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanco(int id)
        {
            var banco = await _context.Banco.FindAsync(id);
            if (banco == null)
            {
                return NotFound();
            }

            _context.Banco.Remove(banco);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<string> UpdateCorebanking(Banco banco, string operationType, string username, string password, string sourceBank, string destinationBank)
        {
            var arrayMessages = _configuration["DiscardedMessages"].Split("|");
            var uri = _configuration["coreBankingUrl"];

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
            ServicePointManager.ServerCertificateValidationCallback = (_, _, _, _) => true;

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            if (uri.ToLower().StartsWith("https"))
            {
                binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            }
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

            EndpointAddress endPointAddress = new(uri);

            var service = new operacionesACHPSClient(binding, endPointAddress);
            service.ChannelFactory.Credentials.ServiceCertificate.SslCertificateAuthentication =
                new X509ServiceCertificateAuthentication
                {
                    CertificateValidationMode = X509CertificateValidationMode.None,
                    RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck,
                };

            PRODUCT_TYPEType[] productType = Array.Empty<PRODUCT_TYPEType>();
            if (banco.PermiteAH)
            {
                productType = productType.Append(new PRODUCT_TYPEType
                {
                    TYPE = "Saving",
                    MAX_LENGTH = $"{banco.AHLongitudMax}",
                    MIN_LENGTH = $"{banco.AHLongitudMin}",
                    LEAD_ZERO = $"{(banco.AHCompletaCeros.HasValue && banco.AHCompletaCeros.Value ? "YES" : "NO")}",
                    MESSAGE_ERROR = $"{banco.AHMensajeError}",
                }).ToArray();
            }
            if (banco.PermiteCC)
            {
                productType = productType.Append(new PRODUCT_TYPEType
                {
                    TYPE = "Checking",
                    MAX_LENGTH = $"{banco.CCLongitudMax}",
                    MIN_LENGTH = $"{banco.CCLongitudMin}",
                    LEAD_ZERO = $"{(banco.CCCompletaCeros.HasValue && banco.CCCompletaCeros.Value ? "YES" : "NO")}",
                    MESSAGE_ERROR = $"{banco.CCMensajeError}",
                }).ToArray();
            }
            if (banco.PermiteTC)
            {
                productType = productType.Append(new PRODUCT_TYPEType
                {
                    TYPE = "Creditcard",
                    MAX_LENGTH = $"{banco.TCLongitudMax}",
                    MIN_LENGTH = $"{banco.TCLongitudMin}",
                    LEAD_ZERO = $"{(banco.TCCompletaCeros.HasValue && banco.TCCompletaCeros.Value ? "YES" : "NO")}",
                    MESSAGE_ERROR = $"{banco.TCMensajeError}",
                }).ToArray();
            }
            if (banco.PermitePTO)
            {
                productType = productType.Append(new PRODUCT_TYPEType
                {
                    TYPE = "Loanpayment",
                    MAX_LENGTH = $"{banco.PTOLongitudMax}",
                    MIN_LENGTH = $"{banco.PTOLongitudMin}",
                    LEAD_ZERO = $"{(banco.PTOCompletaCeros.HasValue && banco.PTOCompletaCeros.Value ? "YES" : "NO")}",
                    MESSAGE_ERROR = $"{banco.PTOMensajeError}",
                }).ToArray();
            }

            BANK_ROUTEType[] route = Array.Empty<BANK_ROUTEType>();

            route = route.Append(new BANK_ROUTEType
            {
                ROUTE_CURRENCY = "HNL",
                ROUTE_NUMBER = banco.CodigoACHMonedaLocal
            }).ToArray();

            route = route.Append(new BANK_ROUTEType
            {
                ROUTE_CURRENCY = "USD",
                ROUTE_NUMBER = banco.CodigoACHMonedaExtranjera
            }).ToArray();


            var result = await service.registraParametrizacionAsync(new RequestHeader
            {
                Authentication = new Authentication
                {
                    UserName = username,
                    Password = password
                },
                Region = new Region
                {
                    SourceBank = sourceBank,
                    DestinationBank = destinationBank
                }
            }, new registraParametrizacion
            {
                OPERATION_TYPE = operationType,
                ID_CEPROBAN = banco.CodigoCoreBancario,
                NAME = banco.NombreBanco,
                PRODUCT_TYPE = productType,
                ROUTE = route,
                BANK_STATUS = banco.Estado,
                ID_BANK_CEPROBAN = banco.CodigoCoreBancario,

            });

            if (arrayMessages.Length > 0 && result.ResponseHeader.successIndicator == "ERROR")
            {
                if (result.ResponseHeader.messages.Any(msgOsb => arrayMessages.Any(msgOsb.Contains)))
                {
                    return string.Empty;
                }
            }

            if (result.ResponseHeader.successIndicator != "ERROR") return result.ResponseHeader.successIndicator;

            var errorMsg = string.Concat(result.ResponseHeader.messages);
            throw new Exception(errorMsg);
        }
    }
}
