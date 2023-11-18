using Conexiones;
using Dominio.Entidades;
using DTOS;
using IRepositorios;
using System.Data.SqlClient;

namespace Repositorios
{
    public class RepositorioProducto : RepositorioBase, IRepositorioProducto
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Alta(DTOProducto obj)
        {
            Producto producto = new Producto();
            producto.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaProducto = @"INSERT INTO Producto VALUES (@Nombre, @Descripcion, @PrecioActual, @PrecioAnterior, @IdTipoProducto, @VisibleEnWeb, @Nuevo, @BajaLogica)
                                    SELECT CAST(Scope_IDentity() as int)";

                
                SqlCommand cmd = new SqlCommand(sentenciaProducto, cn);
                cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                cmd.Parameters.AddWithValue("@PrecioActual", producto.PrecioActual);
                cmd.Parameters.AddWithValue("@PrecioAnterior", producto.PrecioAnterior);
                cmd.Parameters.AddWithValue("@IdTipoProducto", producto.IdTipoProducto);
                cmd.Parameters.AddWithValue("@VisibleEnWeb", true);
                cmd.Parameters.AddWithValue("@Nuevo", false);
                cmd.Parameters.AddWithValue("@BajaLogica", false);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int idGenerado = (int)cmd.ExecuteScalar();

                if (producto.Stocks.Count > 0)
                {
                    foreach (var stock in producto.Stocks)
                    {
                        string sentenciaStock = @"INSERT INTO Stock VALUES(@IdProducto, @IdColor, @IdTalle)
                                            SELECT CAST(Scope_IDentity() as int)";
                        cmd = new SqlCommand(sentenciaStock, cn);
                        cmd.Parameters.AddWithValue("@IdProducto", stock.IdProducto);
                        cmd.Parameters.AddWithValue("@IdColor", stock.IdColor);
                        cmd.Parameters.AddWithValue("@IdTalle", stock.IdTalle);
                        idGenerado = (int)cmd.ExecuteScalar();
                    }
                }

                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return true;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public bool BajaLogica(DTOProducto obj)
        {
            Producto producto = new Producto();
            producto.cargarDeDTO(obj);


            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE TABLE Producto SET bajaLogica = @BajaLogica WHERE idProducto = @IdProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                cmd.Parameters.AddWithValue("@BajaLogica", true);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int idGenerado = cmd.ExecuteNonQuery();
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return idGenerado > 0;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public DTOProducto BuscarPorId(DTOProducto obj)
        {
            Producto producto = new Producto();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Producto WHERE idProducto = @IdProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdProducto", obj.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        producto.Id = Convert.ToInt64(reader["idProducto"]);
                        producto.Nombre = Convert.ToString(reader["nombre"]);
                        producto.Descripcion = Convert.ToString(reader["descripcion"]);
                        producto.PrecioActual = Convert.ToDouble(reader["precioActual"]);
                        producto.PrecioAnterior = Convert.ToDouble(reader["precioAnterior"]);
                        producto.IdTipoProducto = Convert.ToInt64(reader["idTipoProducto"]);
                        producto.VisibleEnWeb = Convert.ToBoolean(reader["visibleEnWeb"]);
                        producto.Nuevo = Convert.ToBoolean(reader["Nuevo"]);
                        producto.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);

                if (producto == null || producto.Id == 0)
                {
                    return null;
                }

                return producto.darDto();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public DTOProducto BuscarPorNombre(DTOProducto dtoProducto)
        {
            Producto producto = new Producto();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Producto WHERE UPPER(nombre) = @Nombre";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Nombre", dtoProducto.Nombre.ToUpper());
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        producto.Id = Convert.ToInt64(reader["idProducto"]);
                        producto.Nombre = Convert.ToString(reader["nombre"]);
                        producto.Descripcion = Convert.ToString(reader["descripcion"]);
                        producto.PrecioActual = Convert.ToDouble(reader["precioActual"]);
                        producto.PrecioAnterior = Convert.ToDouble(reader["precioAnterior"]);
                        producto.IdTipoProducto = Convert.ToInt64(reader["idTipoProducto"]);
                        producto.VisibleEnWeb = Convert.ToBoolean(reader["visibleEnWeb"]);
                        producto.Nuevo = Convert.ToBoolean(reader["Nuevo"]);
                        producto.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);

                if (producto == null || producto.Id == 0)
                {
                    return null;
                }

                return producto.darDto();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public bool Eliminar(DTOProducto obj)
        {
            Producto producto = new Producto();
            producto.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaAlerta = @"DELETE FROM AlertaStock WHERE idStock IN (SELECT idStock FROM Stock WHERE idProducto = @IdProducto)";
                string sentenciaStock = @"DELETE From Stock WHERE idProducto = @IdProducto";
                string sentenciaProducto = @"DELETE FROM Producto WHERE idProducto = @IdProducto";

                SqlCommand cmd = new SqlCommand(sentenciaAlerta, cn);
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int affected = cmd.ExecuteNonQuery();

                cmd = new SqlCommand(sentenciaStock, cn);
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                affected = cmd.ExecuteNonQuery();

                cmd = new SqlCommand(sentenciaProducto, cn);
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                affected = cmd.ExecuteNonQuery();

                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return true;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public bool EnUso(DTOProducto dtoProducto)
        {
            Producto producto = new Producto();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT TOP 1 idVenta FROM VentaProducto WHERE idProducto = @IdProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdProducto", dtoProducto.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        producto.Id = Convert.ToInt64(reader["idProducto"]);
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return producto != null && producto.Id > 0;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public bool Modificar(DTOProducto obj)
        {
            Producto producto = new Producto();
            producto.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE Producto SET nombre = @Nombre, descripcion = @Descripcion, precioActual = @PrecioActual, precioAnterior = @PrecioAnterior, idTipoProducto = @IdTipoProducto, visibleEnWeb = @VisibleEnWeb, nuevo = @Nuevo WHERE idProducto = @IdProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                cmd.Parameters.AddWithValue("@PrecioActual", producto.PrecioActual);
                cmd.Parameters.AddWithValue("@PrecioAnterior", producto.PrecioAnterior);
                cmd.Parameters.AddWithValue("@IdTipoProducto", producto.IdTipoProducto);
                cmd.Parameters.AddWithValue("@VisibleEnWeb", producto.VisibleEnWeb);
                cmd.Parameters.AddWithValue("@Nuevo", producto.Nuevo);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int idGenerado = cmd.ExecuteNonQuery();
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return true;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public IEnumerable<DTOProducto> TraerTodos()
        {
            List<DTOProducto> productos = new List<DTOProducto>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Producto WHERE bajaLogica = 0";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Producto producto = new Producto();

                        producto.Id = Convert.ToInt64(reader["idProducto"]);
                        producto.Nombre = Convert.ToString(reader["nombre"]);
                        producto.Descripcion = Convert.ToString(reader["descripcion"]);
                        producto.PrecioActual = Convert.ToDouble(reader["precioActual"]);
                        producto.PrecioAnterior = Convert.ToDouble(reader["precioAnterior"]);
                        producto.IdTipoProducto = Convert.ToInt64(reader["idTipoProducto"]);
                        producto.VisibleEnWeb = Convert.ToBoolean(reader["visibleEnWeb"]);
                        producto.Nuevo = Convert.ToBoolean(reader["Nuevo"]);
                        producto.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);

                        DTOProducto dtoTipoT = producto.darDto();

                        productos.Add(dtoTipoT);
                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                return productos;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }
    }
}
