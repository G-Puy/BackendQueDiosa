using DTOS;

namespace IRepositorios
{
    public interface IRepositorioTalle : IRepositorioT<DTOTalle>
    {
        DTOTalle BuscarPorNombre(DTOTalle dtoTalle);

        public bool EnUso(DTOTalle dtoTalle);
    }
}
