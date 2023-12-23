namespace Ficohsa.Models.Auditable
{
    public static class AuditableExtensions
    {
        public static void CreateSave(this IAuditableEntity auditable)
        {
            auditable.FechaCreacion = DateTime.Now;
            //auditable.UsuarioCreacion = "Jeyko";
            auditable.UsuarioModificacion = null;
        }

        public static void UpdateSave(this IAuditableEntity auditable)
        {
            auditable.FechaModificacion = DateTime.Now;
            //auditable.UsuarioModificacion = "Jeyko";
        }
    }
}
