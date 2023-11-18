using Conexiones;
using Dominio.Entidades;
using DTOS;
using IRepositorios;
using System.Data.SqlClient;

namespace Repositorios
{
    public class RepositorioStock : RepositorioBase, IRepositorioStock
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Alta(DTOStock obj)
        {
            Stock stock = new Stock();
            stock.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaProducto = @"INSERT INTO Stock VALUES(@IdProducto, @IdColor, @IdTalle)
                                            SELECT CAST(Scope_IDentity() as int)";
                SqlCommand cmd = new SqlCommand(sentenciaProducto, cn);
                cmd.Parameters.AddWithValue("@IdProducto", stock.IdProducto);
                cmd.Parameters.AddWithValue("@IdColor", stock.IdColor);
                cmd.Parameters.AddWithValue("@IdTalle", stock.IdTalle);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int idGenerado = (int)cmd.ExecuteScalar();
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

        public bool BajaLogica(DTOStock obj)
        {
            throw new NotImplementedException();
        }

        public DTOStock BuscarPorId(DTOStock obj)
        {
            Stock stock = new Stock();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Stock WHERE idStock = @IdStock";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdStock", obj.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stock.Id = Convert.ToInt64(reader["idStock"]);
                        stock.IdProducto = Convert.ToInt64(reader["idProducto"]);
                        stock.IdColor = Convert.ToInt64(reader["idColor"]);
                        stock.IdTalle = Convert.ToInt64(reader["idTalle"]);
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);

                if (stock == null || stock.Id == 0)
                {
                    return null;
                }

                return stock.darDto();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public bool Eliminar(DTOStock obj)
        {
            Stock stock = new Stock();
            stock.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaAlerta = @"DELETE FROM AlertaStock WHERE idStock = @IdStock";
                string sentenciaStock = @"DELETE FROM Stock WHERE idStock = @IdStock";

                SqlCommand cmd = new SqlCommand(sentenciaAlerta, cn);
                cmd.Parameters.AddWithValue("@IdStock", stock.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int affected = cmd.ExecuteNonQuery();

                cmd = new SqlCommand(sentenciaStock, cn);
                cmd.Parameters.AddWithValue("@IdStock", stock.Id);
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

        public bool Modificar(DTOStock obj)
        {
            Stock stock = new Stock();
            stock.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE Stock SET idProducto = @IdProducto, idColor = @IdColor, idTalle = @IdTalle WHERE idStock = @IdStock";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdStock", stock.Id);
                cmd.Parameters.AddWithValue("@IdProducto", stock.IdProducto);
                cmd.Parameters.AddWithValue("@IdColor", stock.IdColor);
                cmd.Parameters.AddWithValue("@IdTalle", stock.IdTalle);
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

        public IEnumerable<DTOStock> TraerTodos()
        {
            List<DTOStock> stocks = new List<DTOStock>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Stock";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DTOStock stock = new DTOStock();
                        stock.Id = Convert.ToInt64(reader["idStock"]);
                        stock.IdProducto = Convert.ToInt64(reader["idProducto"]);
                        stock.IdColor = Convert.ToInt64(reader["idColor"]);
                        stock.IdTalle = Convert.ToInt64(reader["idTalle"]);
                        stocks.Add(stock);
                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                return stocks;
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
