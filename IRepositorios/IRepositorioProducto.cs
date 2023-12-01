using Dominio.Entidades;
using DTOS;
using Microsoft.AspNetCore.Http;

namespace IRepositorios
{
    public interface IRepositorioProducto
    {

        Task<bool> Alta(DTOProducto obj, IFormFileCollection imagenes);

        Task<bool> Eliminar(DTOProducto obj);

        Task<bool> Modificar(DTOProducto obj, IFormFileCollection imagenes);

        Task<DTOProducto> BuscarPorId(DTOProducto obj);

        Task<IEnumerable<DTOProducto>> TraerTodos();

        bool BajaLogica(DTOProducto obj);

        Task<DTOProducto> BuscarPorNombre(DTOProducto dtoProducto);

        bool EnUso(DTOProducto dtoProducto);
    }
}
