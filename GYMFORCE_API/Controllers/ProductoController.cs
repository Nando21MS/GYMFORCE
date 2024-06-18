using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GYMFORCE_API.Models;
using GYMFORCE_API.Repositorio.DAO;

namespace GYMFORCE_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : Controller
    {

        [HttpGet("listadoProductoPorProveedor/{proveedorId}")]
        public async Task<ActionResult<List<Producto>>> listadoProductoPorProveedor(int proveedorId)
        {
            var lista = await Task.Run(() => new ProductoDAO().listadoProductoPorProveedor(proveedorId));
            return Ok(lista);
        }

        [HttpGet("listadoProducto")]
        public async Task<ActionResult<List<Producto>>> listadoProducto()
        {
            var lista = await Task.Run(() => new ProductoDAO().listadoProducto());
            return Ok(lista);
        }

        [HttpGet("listadoProductoO")]
        public async Task<ActionResult<List<ProductoO>>> listadoProductoO()
        {
            var lista = await Task.Run(() => new ProductoDAO().listadoProductoO());
            return Ok(lista);
        }

        [HttpGet("listadoCategoria")]
        public async Task<ActionResult<List<Categoria>>> listadoCategoria()
        {
            var lista = await Task.Run(() =>
            new CategoriaDAO().listadoCategoria());
            return Ok(lista);
        }

        [HttpPost("nuevoProducto")]
        public async Task<ActionResult<string>> nuevoProducto(ProductoO objP)
        {
            var mensaje = await Task.Run(() =>
                          new ProductoDAO().nuevoProducto(objP));
            return Ok(mensaje);
        }

        [HttpPut("modificaProducto")]
        public async Task<ActionResult<string>> modificaProducto(ProductoO objP)
        {
            var mensaje = await Task.Run(() =>
                          new ProductoDAO().modificaProducto(objP));
            return Ok(mensaje);
        }

        [HttpGet("buscarProducto/{id}")]
        public async Task<ActionResult<List<ProductoO>>> buscarProducto(int id)
        {
            var lista = await Task.Run(() => new ProductoDAO().buscarProducto(id));
            return Ok(lista);
        }


        //PARA REPORTE
        [HttpGet("reporteProducto")]
        public async Task<ActionResult<List<Producto>>> reporteProducto(
        [FromQuery] string nombre = null,
        [FromQuery] int? categoria = null,
        [FromQuery] int? stock = null,
        [FromQuery] int? proveedor = null)
        {
            var lista = await Task.Run(() => new ProductoDAO().reporteProducto(nombre, categoria, stock, proveedor));
            return Ok(lista);
        }
        //FIN DE REPORTE

    }
}
