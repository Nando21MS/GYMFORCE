using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Presentacion.Models
{
    public class Proveedor
    {
        [DisplayName("CODIGO")]
        public int id_proveedor { get; set; }

        [Required(ErrorMessage = "NOMBRE DEL PROVEEDOR")]
        [DisplayName("PROVEEDOR")]
        public string? raz_soc { get; set; }

        [Required(ErrorMessage = "RUC DEL PROVEEDOR")]
        [DisplayName("RUC")]
        public string? ruc { get; set; }

        [Required(ErrorMessage = "TELEFONO DEL PROVEEDOR")]
        [DisplayName("TELEFONO")]
        public string? telefono { get; set; }

        [Required(ErrorMessage = "CORREO DEL PROVEEDOR")]
        [DisplayName("CORREO")]
        public string? correo { get; set; }

        [Required(ErrorMessage = "DIRECCION DEL PROVEEDOR")]
        [DisplayName("DIRECCION")]
        public string? direccion { get; set; }

        [Required(ErrorMessage = "FOTO DEL PROVEEDOR")]
        [DisplayName("FOTO DEL PROVEEDOR")]
        public string? foto_prov { get; set; }
    }
}
