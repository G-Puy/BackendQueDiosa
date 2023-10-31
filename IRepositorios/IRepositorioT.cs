namespace IRepositorios
{
    public interface IRepositorioT<T>
    {
        bool Alta(T obj);

        bool Eliminar(T obj);

        bool Modificar(T obj);

        T BuscarPorId(int id);

        IEnumerable<T> TraerTodos();
        bool BajaLogica(T obj);
    }
}