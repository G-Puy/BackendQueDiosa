namespace BackendQueDiosa.Mappers
{
    public class MapperUsuarioAlta
    {
        public string NombreDeUsuario { get; set; } = "";
        public string Contraseña { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Correo { get; set; } = "";
        public int IdTipoUsuario { get; set; } = 0;
    }
}
