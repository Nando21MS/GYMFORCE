﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Proyecto.Presentacion.Models;
using System.Text;
using ClosedXML.Excel;

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
                return RedirectToAction(nameof(listadoProductoPorProveedor));
            }

            // Asignar el proveedorId a ViewBag para usarlo en la vista
            ViewBag.proveedorId = proveedorId;

            return View("listadoProductoPorProveedor", productosPorProveedor);
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
                return RedirectToAction(nameof(listadoProductoPorProveedor));
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
                // Obtener el proveedorId del producto modificado
                var proveedorId = objP.id_proveedor;
                return RedirectToAction(nameof(listadoProductoPorProveedor), new { proveedorId });
            }
            else
            {
                TempData["ErrorMessage"] = "Error al actualizar el producto.";
            }

            ViewBag.categoria = new SelectList(aCategoria(), "id_categoria", "nom_cat", objP.id_categoria);
            ViewBag.proveedor = new SelectList(aProveedores(), "id_proveedor", "raz_soc", objP.id_proveedor);
            return View(objP);
        }

        //PARA REPORTE
        [HttpGet]
        public IActionResult reporteProducto(string nombre = null, int? id_categoria = null, int? stock = null, int? id_proveedor = null)
        {
            List<Producto> aProducto = new List<Producto>();

            // Construir la cadena de consulta dinámicamente
            var queryParameters = new List<string>();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryParameters.Add($"nombre={nombre}");
            }
            if (id_categoria.HasValue)
            {
                queryParameters.Add($"categoria={id_categoria.Value}");
            }
            if (stock.HasValue)
            {
                queryParameters.Add($"stock={stock.Value}");
            }
            if (id_proveedor.HasValue)
            {
                queryParameters.Add($"proveedor={id_proveedor.Value}");
            }
            var query = string.Join("&", queryParameters);

            // Si hay parámetros, añadir el prefijo "?"
            if (!string.IsNullOrEmpty(query))
            {
                query = "?" + query;
            }

            HttpResponseMessage response =
                _httpClient.GetAsync(_httpClient.BaseAddress + "/Producto/reporteProducto" + query).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                aProducto = JsonConvert.DeserializeObject<List<Producto>>(data);
            }

            // Obtener la lista de categorías
            List<Categoria> categorias = aCategoria();

            // Pasar los valores actuales de los filtros a ViewBag
            ViewBag.CurrentFilterNombre = nombre;
            ViewBag.CurrentFilterCategoria = id_categoria;
            ViewBag.CurrentFilterStock = stock;
            ViewBag.CurrentFilterProveedor = id_proveedor;


            // Pasar las categorías y la lista de productos a la vista
            ViewBag.categoria = new SelectList(categorias, "id_categoria", "nom_cat");
            ViewBag.proveedor = new SelectList(aProveedores(), "id_proveedor", "raz_soc");
            return View(aProducto);
        }
        [HttpPost]
        public IActionResult GenerarExcel(string nombre = null, int? id_categoria = null, int? stock = null, int? id_proveedor = null)
        {
            // Obtener los datos filtrados
            List<Producto> productosFiltrados = ObtenerProductosFiltrados(nombre, id_categoria, stock, id_proveedor);

            // Crear el libro de Excel y la hoja de trabajo
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Productos");

            // Agregar nombres de encabezados sin estilo
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Nombre";
            worksheet.Cell(1, 3).Value = "Descripción";
            worksheet.Cell(1, 4).Value = "Categoría";
            worksheet.Cell(1, 5).Value = "Precio";
            worksheet.Cell(1, 6).Value = "Stock";
            worksheet.Cell(1, 7).Value = "Proveedor";
            worksheet.Cell(1, 8).Value = "Foto";
            worksheet.Cells().Style.Border.OutsideBorder = XLBorderStyleValues.Thin; // Borde delgado en el exterior de la celda
            worksheet.Cells().Style.Border.InsideBorder = XLBorderStyleValues.Thin; // Borde delgado dentro de la celda

            // Estilo para todos los encabezados
            var headerStyle = worksheet.Range("A1:H1").Style;
            headerStyle.Font.Bold = true; // Texto en negrita
            headerStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // Alineación horizontal centrada
            headerStyle.Fill.BackgroundColor = XLColor.LightSkyBlue; // Color de fondo celeste claro
            headerStyle.Font.FontColor = XLColor.Black; // Color de fuente negro
            headerStyle.Alignment.Vertical = XLAlignmentVerticalValues.Center; // Alineación vertical centrada

            // Agregar datos filtrados al archivo Excel
            for (int i = 0; i < productosFiltrados.Count; i++)
            {
                var row = worksheet.Row(i + 2); // Comienza desde la segunda fila (encabezados en la primera fila)

                row.Cell(1).Value = productosFiltrados[i].id_producto;
                row.Cell(2).Value = productosFiltrados[i].nom_prod;
                row.Cell(3).Value = productosFiltrados[i].des_prod;
                row.Cell(4).Value = productosFiltrados[i].nom_cat;
                row.Cell(5).Value = productosFiltrados[i].pre_prod;
                row.Cell(6).Value = productosFiltrados[i].stock;
                row.Cell(7).Value = productosFiltrados[i].raz_soc;
                row.Cell(8).Value = productosFiltrados[i].foto_prod;

                // Agregar bordes a la celda
                row.Cells().Style.Border.OutsideBorder = XLBorderStyleValues.Thin; // Borde delgado en el exterior de la celda
                row.Cells().Style.Border.InsideBorder = XLBorderStyleValues.Thin; // Borde delgado dentro de la celda

                // Ajustar el ancho de las celdas al contenido
                for (int j = 1; j <= 8; j++)
                {
                    row.Cell(j).Style.Alignment.WrapText = true; // Ajuste automático de texto
                    if (j != 2 && j != 3) // No centrar "Nombre" y "Descripción"
                    {
                        row.Cell(j).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // Alineación horizontal centrada
                        row.Cell(j).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center; // Alineación vertical centrada
                    }
                }
            }

            // Ajustar el ancho de las columnas al contenido
            worksheet.Columns().AdjustToContents();

            // Guardar el libro de Excel en un MemoryStream
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                // Devolver el archivo Excel como una descarga
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Productos.xlsx");
            }
        }

        private List<Producto> ObtenerProductosFiltrados(string nombre, int? id_categoria, int? stock, int? id_proveedor)
        {
            // Construir la cadena de consulta dinámicamente
            var queryParameters = new List<string>();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryParameters.Add($"nombre={nombre}");
            }
            if (id_categoria.HasValue)
            {
                queryParameters.Add($"categoria={id_categoria.Value}");
            }
            if (stock.HasValue)
            {
                queryParameters.Add($"stock={stock.Value}");
            }
            if (id_proveedor.HasValue)
            {
                queryParameters.Add($"proveedor={id_proveedor.Value}");
            }
            var query = string.Join("&", queryParameters);

            // Si hay parámetros, añadir el prefijo "?"
            if (!string.IsNullOrEmpty(query))
            {
                query = "?" + query;
            }

            // Realizar la llamada a la API para obtener los datos filtrados
            HttpResponseMessage response =
                _httpClient.GetAsync(_httpClient.BaseAddress + "/Producto/reporteProducto" + query).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Producto>>(data);
            }

            // En caso de error o si no se obtienen datos, devolver una lista vacía o manejar el error según sea necesario
            return new List<Producto>();
        }


        //FIN DE REPORTE

        public IActionResult Index()
        {
            return View();
        }
    }
}
