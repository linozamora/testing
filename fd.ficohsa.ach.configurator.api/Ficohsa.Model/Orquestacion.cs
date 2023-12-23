namespace Ficohsa.Models
{
    public class Orquestacion
    {
        public int IdOrquestacion { get; set; }
        public int IdPais { get; set; }
        public string Direccion { get; set; } = null!;
        public string EstadoActual { get; set; } = null!;
        public string SiguientePaso { get; set; } = null!;
        public string Topico { get; set; } = null!;
        public int? Retraso { get; set; }
        public int? Reintentos { get; set; }
        public int? RetrasoExtendido { get; set; }
        public int? ReintentosExtendido { get; set; }
        public string? EstadoFallido { get; set; }
        public string? EstadoRecuperacion { get; set; }
        public string? Monto { get; set; }
        public string? PILocal { get; set; }
        public string? Canal { get; set; }
        public string? Producto { get; set; }
        public string? BancoDestino { get; set; }
        public string? MessageType { get; set; }

        public virtual Pais? IdPaisNavigation { get; set; } 
    }
}
