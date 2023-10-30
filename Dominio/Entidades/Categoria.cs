namespace Dominio.Entidades
{
    public class Categoria
    {
        public long IdCategoria { get; set; } = 0;
        public string NombreCategoria { get; set; } = "";
        public bool BajaLogica { get; set; } = false;
        public Categoria() { }
    }
}