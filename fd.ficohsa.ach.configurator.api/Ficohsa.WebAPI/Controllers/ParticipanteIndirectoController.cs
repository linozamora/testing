using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ficohsa.Models;
using Ficohsa.DAO;
using Ficohsa.Models.Auditable;
using Microsoft.Net.Http.Headers;
using ServiceReference;
using System.Text;
using System.ServiceModel;
using System.Net;
using System.ServiceModel.Security;
using System;

namespace Ficohsa.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipanteIndirectoController : ControllerBase
    {
        private readonly FicohsaContext _context;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ParticipanteIndirectoController(FicohsaContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // GET: api/ParticipanteIndirecto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipanteIndirecto>>> GetParticipanteIndirectoList(int idPais)
        {
            var lstParticipanteIndirecto = await _context.ParticipanteIndirecto
                .Include(p => p.Canal)
                .Include(p => p.Banco)
                .Where(p => p.IdPais == idPais)
                .ToListAsync();
            lstParticipanteIndirecto.ForEach(p =>
            {
                p.APIPassword = Decrypt(p.APIPassword ?? string.Empty);
            });
            return lstParticipanteIndirecto;
        }

        // GET: api/ParticipanteIndirecto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipanteIndirecto>> GetParticipanteIndirecto(int id)
        {
            var participanteIndirecto = await _context.ParticipanteIndirecto.FindAsync(id);

            if (participanteIndirecto == null)
            {
                return NotFound();
            }
            else
                participanteIndirecto.APIPassword = Decrypt(participanteIndirecto.APIPassword ?? string.Empty);


            return participanteIndirecto;
        }

        // PUT: api/ParticipanteIndirecto/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipanteIndirecto(int id, ParticipanteIndirecto participanteIndirecto)
        {
            if (id != participanteIndirecto.IdParticipanteIndirecto)
            {
                return BadRequest();
            }

            string sourceBank;
            string destinationBank;
            string userName;
            string password;

            try
            {
                var parameters = _context.Parametro.ToList();
                sourceBank = parameters.Single(p => p.Nombre == _configuration["Sourcebank"]).Valor;
                destinationBank = parameters.Single(p => p.Nombre == _configuration["DestinationBank"]).Valor;
                userName = parameters.Single(p => p.Nombre == _configuration["UsernameOSB"]).Valor;
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
                var country = await _context.Pais.FirstAsync(p => p.IdPais == participanteIndirecto.IdPais);
                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Post,
                    "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateIndirectParticipants?countryCode=" + country.CodigoISO)
                {
                    Headers =
                    {
                        { HeaderNames.Accept, "application/json" },
                    },

                };
                var httpClient = _httpClientFactory.CreateClient();
                await httpClient.SendAsync(httpRequestMessage);

                //saving the db record
                participanteIndirecto.APIPassword = Crypt(participanteIndirecto.APIPassword ?? string.Empty);
                participanteIndirecto.UpdateSave();
                _context.Entry(participanteIndirecto).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                //updating the core banking record type "PI"
                _ = await UpdateCorebanking(participanteIndirecto, "PI", userName, password, sourceBank, destinationBank);
                try
                {
                    _ = await UpdateCorebanking(participanteIndirecto, "PI.Accounts", userName, password, sourceBank, destinationBank);
                }
                catch (Exception e)
                {
                    await dbTransaction.RollbackAsync();
                    try
                    {
                        participanteIndirecto = (from x in _context.ParticipanteIndirecto
                                                 where x.IdParticipanteIndirecto == participanteIndirecto.IdParticipanteIndirecto &&
                                                       x.IdPais == participanteIndirecto.IdPais
                                                 select x).Single();
                        _ = await UpdateCorebanking(participanteIndirecto, "PI", userName, password, sourceBank, destinationBank);
                    }
                    catch
                    {
                        // ignored
                    }
                    return BadRequest(e.Message);
                }

                try
                {
                    _ = await UpdateCorebanking(participanteIndirecto, "PI.Masivos", userName, password, sourceBank, destinationBank);
                }
                catch
                {
                    // ignored
                }
                await dbTransaction.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Error al actualizar registro {ex.InnerException?.Message ?? string.Empty}");
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // POST: api/ParticipanteIndirecto
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ParticipanteIndirecto>> PostParticipanteIndirecto(ParticipanteIndirecto participanteIndirecto)
        {
            string sourceBank;
            string destinationBank;
            string userName;
            string password;

            try
            {
                var parameters = _context.Parametro.ToList();
                sourceBank = parameters.Single(p => p.Nombre == _configuration["Sourcebank"]).Valor;
                destinationBank = parameters.Single(p => p.Nombre == _configuration["DestinationBank"]).Valor;
                userName = parameters.Single(p => p.Nombre == _configuration["UsernameOSB"]).Valor;
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
                var country = await _context.Pais.FirstAsync(p => p.IdPais == participanteIndirecto.IdPais);
                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Post,
                    "http://data-provider-ach-data-provider.ach.svc.cluster.local/DataCache/UpdateIndirectParticipants?countryCode=" + country.CodigoISO)
                {
                    Headers =
                    {
                        { HeaderNames.Accept, "application/json" },
                    },

                };
                var httpClient = _httpClientFactory.CreateClient();
                await httpClient.SendAsync(httpRequestMessage);

                //saving the db record
                participanteIndirecto.APIPassword = Crypt(participanteIndirecto.APIPassword ?? string.Empty);
                participanteIndirecto.CreateSave();
                _context.ParticipanteIndirecto.Add(participanteIndirecto);
                await _context.SaveChangesAsync();

                //updating the core banking record type "PI"
                _ = await UpdateCorebanking(participanteIndirecto, "PI", userName, password, sourceBank, destinationBank);

                try
                {
                    _ = await UpdateCorebanking(participanteIndirecto, "PI.Accounts", userName, password, sourceBank, destinationBank);
                }
                catch (Exception e)
                {
                    await dbTransaction.RollbackAsync();
                    try
                    {
                        participanteIndirecto = (from x in _context.ParticipanteIndirecto
                                                 where x.IdParticipanteIndirecto == participanteIndirecto.IdParticipanteIndirecto &&
                                                       x.IdPais == participanteIndirecto.IdPais
                                                 select x).Single();
                        _ = await UpdateCorebanking(participanteIndirecto, "PI", userName, password, sourceBank, destinationBank);
                    }
                    catch
                    {
                        // ignored
                    }
                    return BadRequest(e.Message);
                }

                try
                {
                    _ = await UpdateCorebanking(participanteIndirecto, "PI.Masivos", userName, password, sourceBank, destinationBank);
                }
                catch
                {
                    // ignored
                }
                await dbTransaction.CommitAsync();
                return CreatedAtAction("GetParticipanteIndirecto", new { id = participanteIndirecto.IdParticipanteIndirecto }, participanteIndirecto);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Error al insertar registro {ex.InnerException?.Message ?? string.Empty}");
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                return BadRequest(ex.Message);
            }

        }

        // DELETE: api/ParticipanteIndirecto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipanteIndirecto(int id)
        {
            var participanteIndirecto = await _context.ParticipanteIndirecto.FindAsync(id);
            if (participanteIndirecto == null)
            {
                return NotFound();
            }

            _context.ParticipanteIndirecto.Remove(participanteIndirecto);
            await _context.SaveChangesAsync();

            return NoContent();
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

        private async Task<string> UpdateCorebanking(
            ParticipanteIndirecto participanteIndirecto,
            string operationType,
            string userName,
            string password,
            string sourceBank,
            string destinationBank)
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
            if (participanteIndirecto.PermiteAH)
            {
                productType = productType.Append(new PRODUCT_TYPEType
                {
                    TYPE = "Saving",
                    MAX_LENGTH = $"{participanteIndirecto.AHLongitudMax}",
                    MIN_LENGTH = $"{participanteIndirecto.AHLongitudMin}",
                    LEAD_ZERO = $"{(participanteIndirecto.AHCompletaCeros.HasValue && participanteIndirecto.AHCompletaCeros.Value ? "YES" : "NO")}",
                    MESSAGE_ERROR = $"{participanteIndirecto.AHMensajeError}",
                }).ToArray();
            }
            if (participanteIndirecto.PermiteCC)
            {
                productType = productType.Append(new PRODUCT_TYPEType
                {
                    TYPE = "Checking",
                    MAX_LENGTH = $"{participanteIndirecto.CCLongitudMax}",
                    MIN_LENGTH = $"{participanteIndirecto.CCLongitudMin}",
                    LEAD_ZERO = $"{(participanteIndirecto.CCCompletaCeros.HasValue && participanteIndirecto.CCCompletaCeros.Value ? "YES" : "NO")}",
                    MESSAGE_ERROR = $"{participanteIndirecto.CCMensajeError}",
                }).ToArray();
            }
            if (participanteIndirecto.PermiteTC)
            {
                productType = productType.Append(new PRODUCT_TYPEType
                {
                    TYPE = "Creditcard",
                    MAX_LENGTH = $"{participanteIndirecto.TCLongitudMax}",
                    MIN_LENGTH = $"{participanteIndirecto.TCLongitudMin}",
                    LEAD_ZERO = $"{(participanteIndirecto.TCCompletaCeros.HasValue && participanteIndirecto.TCCompletaCeros.Value ? "YES" : "NO")}",
                    MESSAGE_ERROR = $"{participanteIndirecto.TCMensajeError}",
                }).ToArray();
            }
            if (participanteIndirecto.PermitePTO)
            {
                productType = productType.Append(new PRODUCT_TYPEType
                {
                    TYPE = "Loanpayment",
                    MAX_LENGTH = $"{participanteIndirecto.PTOLongitudMax}",
                    MIN_LENGTH = $"{participanteIndirecto.PTOLongitudMin}",
                    LEAD_ZERO = $"{(participanteIndirecto.PTOCompletaCeros.HasValue && participanteIndirecto.PTOCompletaCeros.Value ? "YES" : "NO")}",
                    MESSAGE_ERROR = $"{participanteIndirecto.PTOMensajeError}",
                }).ToArray();
            }

            BANK_ROUTEType[] route = Array.Empty<BANK_ROUTEType>();

            route = route.Append(new BANK_ROUTEType
            {
                ROUTE_CURRENCY = "HNL",
                ROUTE_NUMBER = participanteIndirecto.CodigoACHMonedaLocal ?? string.Empty
            }).ToArray();

            route = route.Append(new BANK_ROUTEType
            {
                ROUTE_CURRENCY = "USD",
                ROUTE_NUMBER = participanteIndirecto.CodigoACHMonedaExtranjera ?? string.Empty
            }).ToArray();


            ACCOUNTSType[] accounts = Array.Empty<ACCOUNTSType>();

            accounts = accounts.Append(new ACCOUNTSType
            {
                CURRENCY = "HNL",
                INTER_ACCOUNT = participanteIndirecto.CuentaInternaCoreBancarioMonedaLocal ?? string.Empty,
                CUSTOMER_ACCOUNT = participanteIndirecto.CuentaCoreBancarioMonedaLocal ?? string.Empty
            }).ToArray();

            accounts = accounts.Append(new ACCOUNTSType
            {
                CURRENCY = "USD",
                INTER_ACCOUNT = participanteIndirecto.CuentaInternaCoreBancarioMonedaExtranjera ?? string.Empty,
                CUSTOMER_ACCOUNT = participanteIndirecto.CuentaCoreBancarioMonedaExtranjera ?? string.Empty
            }).ToArray();

            var input = new registraParametrizacion
            {
                OPERATION_TYPE = operationType,
                ID_CEPROBAN = participanteIndirecto.CodigoCoreBancario,
                NAME = participanteIndirecto.Nombre,
                PRODUCT_TYPE = productType,
                ROUTE = route,
                BANK_STATUS = participanteIndirecto.Estado,
                ID_BANK_CEPROBAN = _context.Banco.Where(p => p.IdBanco == participanteIndirecto.IdBanco).Single().CodigoCoreBancario,
                ACCOUNTS = accounts
            };

            var result = await service.registraParametrizacionAsync(new RequestHeader
            {
                Authentication = new Authentication
                {
                    UserName = userName,
                    Password = password
                },
                Region = new Region
                {
                    SourceBank = sourceBank,
                    DestinationBank = destinationBank
                }
            }, input);

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
