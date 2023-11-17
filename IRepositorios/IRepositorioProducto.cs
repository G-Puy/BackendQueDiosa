using Dominio.Entidades;
using DTOS;

namespace IRepositorios
{
    public interface IRepositorioProducto : IRepositorioT<DTOProducto>
    {
        DTOProducto BuscarPorNombre(DTOProducto dtoProducto);

        public bool EnUso(DTOProducto dtoProducto);
    }
}
