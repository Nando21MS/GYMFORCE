using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Proyecto.Presentacion.Models;
using System.Text;

namespace Proyecto.Presentacion.Controllers
{
    public class ProductoController : Controller
    {
    
        //Cadena conexion hacia el servicio
        Uri baseAddress = new Uri("https://localhost:7122/api");
        private readonly HttpClient _httpClient;

        public ProductoController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

 


        [HttpGet]
        public IActionResult listadoProducto()
        {
            List<Producto> aProductos = new List<Producto>();
            HttpResponseMessage response =
                _httpClient.GetAsync(_httpClient.BaseAddress + "/Producto/listadoProducto").Result;
            if (response.IsSuccessStatusCode)
                
            {
                var data = response.Content.ReadAsStringAsync().Result;
                aProductos = JsonConvert.DeserializeObject<List<Producto>>(data);
            }
            return View(aProductos);
        }



        public async Task<IActionResult> listadoProductoPorProveedor(int proveedorId)
        {
            List<Producto> productosPorProveedor = new List<Producto>();

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Producto/listadoProductoPorProveedor/{proveedorId}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                productosPorProveedor = JsonConvert.DeserializeObject<List<Producto>>(data);
            }
            else
            {
                // Manejar el error en caso de que la llamada no sea exitosa
                TempData["ErrorMessage"] = "Error al obtener productos por proveedor.";
                return RedirectToAction(nameof(listadoProducto));
            }

            return View("listadoProducto", productosPorProveedor);
        }




        public List<Categoria> aCategoria()
        {
            List<Categoria> aCategoria = new List<Categoria>();
            HttpResponseMessage response =
            _httpClient.GetAsync(_httpClient.BaseAddress + "/Producto/listadoCategoria").Result;
            var data = response.Content.ReadAsStringAsync().Result;
            aCategoria = JsonConvert.DeserializeObject<List<Categoria>>(data);
            return aCategoria;
        }

        public List<Proveedor> aProveedores()
        {
            List<Proveedor> aProveedores = new List<Proveedor>();
            HttpResponseMessage response =
            _httpClient.GetAsync(_httpClient.BaseAddress + "/Proveedor/listadoProveedor").Result;
            var data = response.Content.ReadAsStringAsync().Result;
            aProveedores = JsonConvert.DeserializeObject<List<Proveedor>>(data);
            return aProveedores;
        }

        [HttpGet]
        public IActionResult nuevoProducto()
        {
            ViewBag.categoria = new SelectList(aCategoria(), "id_categoria", "nom_cat");
            ViewBag.proveedor = new SelectList(aProveedores(), "id_proveedor", "raz_soc");
            return View(new ProductoO());
        }

        [HttpPost]
        public async Task<IActionResult> nuevoProducto(ProductoO objP)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.categoria = new SelectList(aCategoria(), "id_categoria", "nom_cat");
                ViewBag.proveedor = new SelectList(aProveedores(), "id_proveedor", "raz_soc");
                return View(objP);
            }

            var json = JsonConvert.SerializeObject(objP);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var responseC = await _httpClient.PostAsync("/api/Producto/nuevoProducto", content);

            if (responseC.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Producto registrado correctamente..!!!";
                return RedirectToAction(nameof(nuevoProducto));
            }
            else
            {
                TempData["ErrorMessage"] = "Error en el registro del producto.";
            }

            ViewBag.categoria = new SelectList(aCategoria(), "id_categoria", "nom_cat");
            ViewBag.proveedor = new SelectList(aProveedores(), "id_proveedor", "raz_soc");
            return View(objP);
        }



        [HttpGet]
        public async Task<IActionResult> modificarProducto(int id)
        {
            // Obtener detalles del producto desde la API
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Producto/buscarProducto/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var producto = JsonConvert.DeserializeObject<ProductoO>(data);
                ViewBag.categoria = new SelectList(aCategoria(), "id_categoria", "nom_cat", producto.id_categoria);
                ViewBag.proveedor = new SelectList(aProveedores(), "id_proveedor", "raz_soc", producto.id_proveedor);
                return View(producto);
            }
            else
            {
                // Manejar el error de búsqueda del producto
                TempData["ErrorMessage"] = "No se pudo encontrar el producto para modificar.";
                return RedirectToAction(nameof(listadoProducto));
            }
        }

        [HttpPost]
        public async Task<IActionResult> modificarProducto(ProductoO objP)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.categoria = new SelectList(aCategoria(), "id_categoria", "nom_cat", objP.id_categoria);
                ViewBag.proveedor = new SelectList(aProveedores(), "id_proveedor", "raz_soc", objP.id_proveedor);
                return View(objP);
            }

            var json = JsonConvert.SerializeObject(objP);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/Producto/modificaProducto", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Producto actualizado correctamente..!!!";
                return RedirectToAction(nameof(listadoProducto));
            }
            else
            {
                TempData["ErrorMessage"] = "Error al actualizar el producto.";
            }

            ViewBag.categoria = new SelectList(aCategoria(), "id_categoria", "nom_cat", objP.id_categoria);
            ViewBag.proveedor = new SelectList(aProveedores(), "id_proveedor", "raz_soc", objP.id_proveedor);
            return View(objP);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
