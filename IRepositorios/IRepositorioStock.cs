using DTOS;
using System.Data.SqlClient;

namespace IRepositorios
{
    public interface IRepositorioStock : IRepositorioT<DTOStock>
    {
        public bool Modificar(List<DTOStock> obj);

        public bool TieneStock(DTOStock obj);

        public long ActualizarStockYCrearVenta(List<DTOStock> obj, DTOVenta dto);
    }
}
