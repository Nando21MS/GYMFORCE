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
            {
                // Obtener la ruta de la carpeta wwwroot/img/FOTOPRODUCTO en Proyecto.Presentacion
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Proyecto.Presentacion", "wwwroot", "img", "FOTOPRODUCTO");

                // Verificar si la carpeta FOTOPRODUCTO en Proyecto.Presentacion existe, si no, crearla
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Obtener la ruta completa de la imagen a guardar en Proyecto.Presentacion
                var imagePath = Path.Combine(folderPath, $"{objP.nom_prod}.JPG");

                // Convertir la cadena base64 en bytes y guardar la imagen en Proyecto.Presentacion
                byte[] imageBytes = Convert.FromBase64String(objP.foto_prod);
                System.IO.File.WriteAllBytes(imagePath, imageBytes);

                // Actualizar la propiedad foto_prov con la ruta relativa en Proyecto.Presentacion
                objP.foto_prod = $"~/FOTOPRODUCTO/{objP.nom_prod}.JPG";

                // Llamar al método nuevoProducto de ProductoDAO
                return new ProductoDAO().nuevoProducto(objP);
            });

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
