using GYMFORCE_API.Models;

namespace GYMFORCE_API.Repositorio.Interfaces
{
    public interface IProveedor
    {
        IEnumerable<Proveedor> listadoProveedor();
        Proveedor buscarProveedor(int id);
        string nuevoProveedor(Proveedor objP);
        string modificaProveedor(Proveedor objP);
    }
}
