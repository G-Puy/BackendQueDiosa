using Dominio.Entidades;
using DTOS;
using Microsoft.AspNetCore.Http;

namespace IRepositorios
{
    public interface IRepositorioProducto
    {

        Task<bool> Alta(DTOProducto obj, List<IFormFile> imagenes);

        bool Eliminar(DTOProducto obj);

        bool Modificar(DTOProducto obj, List<IFormFile> imagenes);

        DTOProducto BuscarPorId(DTOProducto obj);

        IEnumerable<DTOProducto> TraerTodos();

        bool BajaLogica(DTOProducto obj);

        DTOProducto BuscarPorNombre(DTOProducto dtoProducto);

        public bool EnUso(DTOProducto dtoProducto);
    }
}
