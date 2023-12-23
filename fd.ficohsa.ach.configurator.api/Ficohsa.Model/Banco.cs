using Ficohsa.Models.Auditable;
using System.ComponentModel.DataAnnotations;

namespace Ficohsa.Models
{
    public class Banco : IAuditableEntity
    {
        public int IdBanco { get; set; }
        public int IdPais { get; set; }
        [MaxLength(16)]
        public string CodigoCoreBancario { get; set; } = null!;
        [MaxLength(64)]
        public string NombreBanco { get; set; } = null!;
        [MaxLength(16)]
        public string CodigoACHMonedaLocal { get; set; } = null!;
        [MaxLength(16)]
        public string CodigoACHMonedaExtranjera { get; set; } = null!;
        public int? AHLongitudMin { get; set; }
        public int? AHLongitudMax { get; set; }
        public bool? AHCompletaCeros { get; set; }
        [MaxLength(128)]
        public string? AHMensajeError { get; set; }
        public int? CCLongitudMin { get; set; }
        public int? CCLongitudMax { get; set; }
        public bool? CCCompletaCeros { get; set; }
        [MaxLength(128)]
        public string? CCMensajeError { get; set; }
        public int? TCLongitudMin { get; set; }
        public int? TCLongitudMax { get; set; }
        public bool? TCCompletaCeros { get; set; }
        [MaxLength(128)]
        public string? TCMensajeError { get; set; }
        public int? PTOLongitudMin { get; set; }
        public int? PTOLongitudMax { get; set; }
        public bool? PTOCompletaCeros { get; set; }
        [MaxLength(128)]
        public string? PTOMensajeError { get; set; }
        public bool PermiteAH { get; set; }
        public bool PermiteCC { get; set; }
        public bool PermiteTC { get; set; }
        public bool PermitePTO { get; set; }
        public bool Migrado { get; set; }
        [MaxLength(16)]
        public string Estado { get; set; } = null!;
        [MaxLength(64)]
        public string UsuarioCreacion { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        [MaxLength(64)]
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
