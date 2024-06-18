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
                          new ProveedorDAO().nuevoProveedor(objP));
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
