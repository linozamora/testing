using Ficohsa.Models.Auditable;
using System.ComponentModel.DataAnnotations;

namespace Ficohsa.Models
{
    public class Pais: IAuditableEntity
    {
        public Pais()
        {
            Canal = new HashSet<Canal>();
            Catalogo = new HashSet<Catalogo>();
            Moneda = new HashSet<Moneda>();
            Orquestacion = new HashSet<Orquestacion>();
            Parametro = new HashSet<Parametro>();
        }

        public int IdPais { get; set; }
        [MaxLength(64)]
        public string Nombre { get; set; } = null!;
        [MaxLength(4)]
        public string CodigoISO { get; set; } = null!;
        [MaxLength(16)]
        public string Estado { get; set; } = null!;
        [MaxLength(64)]
        public string UsuarioCreacion { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        [MaxLength(64)]
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ICollection<Canal> Canal { get; set; }
        public virtual ICollection<Catalogo> Catalogo { get; set; }
        public virtual ICollection<Moneda> Moneda { get; set; }
        public virtual ICollection<Orquestacion> Orquestacion { get; set; }
        public virtual ICollection<Parametro> Parametro { get; set; }
    }
}
