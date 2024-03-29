﻿using DTOS;

namespace IRepositorios
{
    public interface IRepositorioUsuario : IRepositorioT<DTOUsuario>
    {
        DTOUsuario Login(DTOUsuario dtoUsuario);

        DTOUsuario BuscarPorNombre(DTOUsuario obj);

        bool NombreOcupado(DTOUsuario dto);

        bool ModificarPass(DTOUsuario obj);
    }
}
