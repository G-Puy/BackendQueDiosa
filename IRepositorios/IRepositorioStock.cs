using DTOS;

namespace IRepositorios
{
    public interface IRepositorioStock : IRepositorioT<DTOStock>
    {
        public bool Modificar(List<DTOStock> obj);

        public bool TieneStock(DTOStock obj);

        public bool ActualizarStock(List<DTOStock> obj);
    }
}
