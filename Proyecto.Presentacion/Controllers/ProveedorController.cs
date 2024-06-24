using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Proyecto.Presentacion.Models;
using System.Text;

namespace Proyecto.Presentacion.Controllers
{
    public class ProveedorController : Controller
    {
        //Cadena conexion hacia el servicio
        Uri baseAddress = new Uri("https://localhost:7122/api");
        private readonly HttpClient _httpClient;

        public ProveedorController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult listadoProveedor()
        {
            List<Proveedor> aProductos = new List<Proveedor>();
            HttpResponseMessage response =
                _httpClient.GetAsync(_httpClient.BaseAddress + "/Proveedor/listadoProveedor").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                aProductos = JsonConvert.DeserializeObject<List<Proveedor>>(data);
            }

            return View(aProductos);
        }


        public async Task<IActionResult> DetalleProveedor(int id)
        {
            Proveedor proveedor = null;
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Proveedor/detalleProveedor/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                proveedor = JsonConvert.DeserializeObject<Proveedor>(data);
            }

            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }



        [HttpGet]
        public IActionResult nuevoProveedor()
        {
            return View(new Proveedor());
        }

        [HttpPost]
        public async Task<IActionResult> nuevoProveedor(Proveedor objP, IFormFile foto_prov)
        {
            if (foto_prov != null && foto_prov.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    foto_prov.CopyTo(ms);
                    var imageBytes = ms.ToArray();
                    objP.foto_prov = Convert.ToBase64String(imageBytes);
                }
            }
            var json = JsonConvert.SerializeObject(objP);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var responseC = await _httpClient.PostAsync("/api/Proveedor/nuevoProveedor", content);

            if (responseC.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Proveedor registrado correctamente..!!!";
                return RedirectToAction(nameof(listadoProveedor));
            }
            else
            {
                TempData["ErrorMessage"] = "Error en el registro del Proveedor.";
            }
            return View(objP);
        }



        [HttpGet]
        public async Task<IActionResult> actualizaProveedor(int id)
        {
            // Obtener detalles del producto desde la API
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Proveedor/buscarProveedor/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var proveedor = JsonConvert.DeserializeObject<Proveedor>(data);
                return View(proveedor);
            }
            else
            {
                // Manejar el error de búsqueda del producto
                TempData["ErrorMessage"] = "No se pudo encontrar el Proveedor para modificar.";
                return RedirectToAction(nameof(listadoProveedor));
            }
        }

        [HttpPost]
        public async Task<IActionResult> actualizaProveedor(Proveedor objP)
        {

            if (!ModelState.IsValid)
            {
                return View(objP);
            }

            var json = JsonConvert.SerializeObject(objP);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/Proveedor/modificaProveedor", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Proveedor actualizado correctamente..!!!";
                return RedirectToAction(nameof(listadoProveedor));
            }
            else
            {
                TempData["ErrorMessage"] = "Error al actualizar el Proveedor.";
            }
            return View(objP);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
