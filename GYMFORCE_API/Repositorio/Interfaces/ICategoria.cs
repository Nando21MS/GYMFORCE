using GYMFORCE_API.Models;

namespace GYMFORCE_API.Repositorio.Interfaces
{
    public interface ICategoria
    {
        //Llenar el combo en la presentación
        IEnumerable<Categoria> listadoCategoria();
    }
}
