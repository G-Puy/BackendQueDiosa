using Dominio.Entidades;
using DTOS;
using Microsoft.AspNetCore.Http;

namespace IRepositorios
{
    public interface IRepositorioProducto
    {

        Task<bool> Alta(DTOProducto obj, List<IFormFile> imagenes);

        Task<bool> Eliminar(DTOProducto obj);

        Task<bool> Modificar(DTOProducto obj, List<IFormFile> imagenes);

        Task<DTOProducto> BuscarPorId(DTOProducto obj);

        Task<IEnumerable<DTOProducto>> TraerTodos();

        bool BajaLogica(DTOProducto obj);

        Task<DTOProducto> BuscarPorNombre(DTOProducto dtoProducto);

        bool EnUso(DTOProducto dtoProducto);

        //Prueba

        Task<DTOProducto> TraerTodosImagenes(int idProducto);
        Task<bool> InsertarEnBlob(List<IFormFile> imagenes, int idProducto);
    }
}
