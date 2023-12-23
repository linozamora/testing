using Ficohsa.Models.Auditable;
using System.ComponentModel.DataAnnotations;

namespace Ficohsa.Models
{
    public class Catalogo: IAuditableEntity
    {
        public int IdCatalogo { get; set; }
        public int IdTabla { get; set; }
        public int IdPais { get; set; }
        [MaxLength(32)]
        public string Codigo { get; set; } = null!;
        [MaxLength(256)]
        public string Valor { get; set; } = null!;
        [MaxLength(16)]
        public string Estado { get; set; } = null!;
        [MaxLength(64)]
        public string UsuarioCreacion { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        [MaxLength(64)]
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual Pais? IdPaisNavigation { get; set; } 
        public virtual Tabla? IdTablaNavigation { get; set; } 
    }
}
