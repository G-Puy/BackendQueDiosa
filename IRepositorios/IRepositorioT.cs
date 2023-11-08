namespace IRepositorios
{
    public interface IRepositorioT<T>
    {
        bool Alta(T obj);

        bool Eliminar(T obj);

        bool Modificar(T obj);

        T BuscarPorId(T obj);

        IEnumerable<T> TraerTodos();
        bool BajaLogica(T obj);
    }
}