using System.ComponentModel.DataAnnotations.Schema;

namespace GYMFORCE_API.Models
{
    public class Proveedor
    {
        public int id_proveedor { get; set; }
        public string? raz_soc { get; set; }
        public string? ruc { get; set; }
        public string? telefono { get; set; }
        public string? correo { get; set; }
        public string? direccion { get; set; }
        public string? foto_prov { get; set; }
    }
}
