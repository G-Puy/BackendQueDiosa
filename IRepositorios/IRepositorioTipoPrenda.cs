using DTOS;

namespace IRepositorios
{
    public interface IRepositorioTipoPrenda : IRepositorioT<DTOTipoPrenda>
    {
        public bool VerificarExistenciaCategoria(DTOTipoPrenda dtoTipoPrenda);

        public DTOTipoPrenda BuscarPorNombre(DTOTipoPrenda dtoTipoPrenda);

        public bool EnUso(DTOTipoPrenda dtoTipoPrenda);

    }
}
