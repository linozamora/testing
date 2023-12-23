using Ficohsa.Models.Auditable;
using System.ComponentModel.DataAnnotations;

namespace Ficohsa.Models
{
    public class Tabla: IAuditableEntity
    {
        public Tabla()
        {
            Catalogo = new HashSet<Catalogo>();
        }

        public int IdTabla { get; set; }
        [MaxLength(64)]
        public string Nombre { get; set; } = null!;
        [MaxLength(64)]
        public string Descripcion { get; set; } = null!;
        [MaxLength(16)]
        public string Estado { get; set; } = null!;
        [MaxLength(64)]
        public string UsuarioCreacion { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        [MaxLength(64)]
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ICollection<Catalogo> Catalogo { get; set; }
    }
}
