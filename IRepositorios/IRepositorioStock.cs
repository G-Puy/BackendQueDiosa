using DTOS;

namespace IRepositorios
{
    public interface IRepositorioStock : IRepositorioT<DTOStock>
    {
        public bool Modificar(List<DTOStock> obj);
    }
}
