using DTOS;

namespace IRepositorios
{
    public interface IRepositorioTipoPrenda : IRepositorioT<DTOTipoPrenda>
    {
        public DTOTipoPrenda BuscarPorNombre(DTOTipoPrenda dtoTipoPrenda);

        public bool EnUso(DTOTipoPrenda dtoTipoPrenda);

        bool NombreOcupado(DTOTipoPrenda dto);

    }
}
