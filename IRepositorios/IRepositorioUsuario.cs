using DTOS;

namespace IRepositorios
{
    public interface IRepositorioUsuario : IRepositorioT<DTOUsuario>
    {
        DTOUsuario Login(DTOUsuario dtoUsuario);
    }
}
