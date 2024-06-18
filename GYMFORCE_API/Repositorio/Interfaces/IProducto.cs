﻿using GYMFORCE_API.Models;

namespace GYMFORCE_API.Repositorio.Interfaces
{
    public interface IProducto
    {
        IEnumerable<Producto> listadoProducto();
        IEnumerable<ProductoO> listadoProductoO();
        ProductoO buscarProducto(int id);
        string nuevoProducto(ProductoO objP);
        string modificaProducto(ProductoO objP);
        //PARA REPORTE
        IEnumerable<Producto> reporteProducto(string nombre = null, int? categoria = null, int? stock = null, int? proveedor = null);
    }
}
