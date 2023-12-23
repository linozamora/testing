using Ficohsa.DAO;
using Ficohsa.Models;
using Ficohsa.RecurringPayments.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ficohsa.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {

        private SecurityRepository _context;

        public SeguridadController()
        {
            _context = new SecurityRepository();
        }

        // GET: api/Tabla
        [HttpGet]
        public Task<ActionResult<IEnumerable<string>>> GetTabla(string user)
        {
            return Task.FromResult<ActionResult<IEnumerable<string>>>(SecurityRepository.GetRolesList(user));
        }
    }
}
