using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GYMFORCE_API.Models;
using GYMFORCE_API.Repositorio.DAO;

namespace GYMFORCE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        [HttpGet("listadoProveedor")]
        public async Task<ActionResult<List<Proveedor>>> listadoProveedor()
        {
            var lista = await Task.Run(() => new ProveedorDAO().listadoProveedor());
            return Ok(lista);
        }

        [HttpPost("nuevoProveedor")]
        public async Task<ActionResult<string>> nuevoProveedor(Proveedor objP)
        {
            var mensaje = await Task.Run(() =>
            {
                // Obtener la ruta de la carpeta wwwroot/img/FOTOPROVEEDOR en Proyecto.Presentacion
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Proyecto.Presentacion", "wwwroot", "img", "FOTOPROVEEDOR");

                // Verificar si la carpeta FOTOPROVEEDOR en Proyecto.Presentacion existe, si no, crearla
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Obtener la ruta completa de la imagen a guardar en Proyecto.Presentacion
                var imagePath = Path.Combine(folderPath, $"{objP.raz_soc}.JPG");

                // Convertir la cadena base64 en bytes y guardar la imagen en Proyecto.Presentacion
                byte[] imageBytes = Convert.FromBase64String(objP.foto_prov);
                System.IO.File.WriteAllBytes(imagePath, imageBytes);

                // Actualizar la propiedad foto_prov con la ruta relativa en Proyecto.Presentacion
                objP.foto_prov = $"~/FOTOPROVEEDOR/{objP.raz_soc}.JPG";

                // Llamar al método nuevoProveedor de ProveedorDAO
                return new ProveedorDAO().nuevoProveedor(objP);
            });

            return Ok(mensaje);
        }

        [HttpPut("modificaProveedor")]
        public async Task<ActionResult<string>> modificaProveedor(Proveedor objP)
        {
            var mensaje = await Task.Run(() =>
                          new ProveedorDAO().modificaProveedor(objP));
            return Ok(mensaje);
        }

        [HttpGet("buscarProveedor/{id}")]
        public async Task<ActionResult<Proveedor>> buscarProveedor(int id)
        {
            var proveedor = await Task.Run(() => new ProveedorDAO().buscarProveedor(id));
            if (proveedor == null)
            {
                return NotFound();
            }
            return Ok(proveedor);
        }

        [HttpGet("detalleProveedor/{id}")]
        public async Task<ActionResult<Proveedor>> detalleProveedor(int id)
        {
            var proveedor = await Task.Run(() => new ProveedorDAO().buscarProveedor(id));
            if (proveedor == null)
            {
                return NotFound();
            }
            return Ok(proveedor);
        }
    }
}
