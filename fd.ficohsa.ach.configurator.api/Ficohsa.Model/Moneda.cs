using Ficohsa.Models.Auditable;
using System.ComponentModel.DataAnnotations;

namespace Ficohsa.Models
{
    public class Moneda: IAuditableEntity
    {
        public int IdMoneda { get; set; }
        public int IdPais { get; set; }
        [MaxLength(32)]
        public string Nombre { get; set; } = null!;
        [MaxLength(4)]
        public string Simbolo { get; set; } = null!;
        [MaxLength(4)]
        public string CodigoISO { get; set; } = null!;
        public decimal TipoCambio { get; set; }
        [MaxLength(16)]
        public string Estado { get; set; } = null!;
        [MaxLength(64)]
        public string UsuarioCreacion { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        [MaxLength(64)]
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual Pais? IdPaisNavigation { get; set; }
    }
}
