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

        public bool ActualizarStock(List<DTOStock> obj)
        {
            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Stock WHERE idProducto = @IdProducto AND idColor = @IdColor AND idTalle = @IdTalle;";

                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;

                List<Stock> stocks = new List<Stock>();

                foreach (DTOStock item in obj)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdProducto", item.IdProducto);
                    cmd.Parameters.AddWithValue("@IdColor", item.IdColor);
                    cmd.Parameters.AddWithValue("@IdTalle", item.IdTalle);

                    Stock stock = new Stock();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stock.Id = Convert.ToInt64(reader["idStock"]);
                            stock.IdProducto = Convert.ToInt64(reader["idProducto"]);
                            stock.IdColor = Convert.ToInt64(reader["idColor"]);
                            stock.IdTalle = Convert.ToInt64(reader["idTalle"]);
                            stock.Cantidad = Convert.ToInt32(reader["cantidad"]);
                        }
                    }

                    stocks.Add(stock);
                }

                foreach (Stock stock in stocks)
                {
                    var objeto = obj.Find(o => o.IdProducto == stock.IdProducto && o.IdTalle == stock.IdTalle && o.IdColor == stock.IdColor);
                    var diferencia = stock.Cantidad - objeto.Cantidad;

                    if (diferencia < 0)
                    {
                        trn.Rollback();
                        manejadorConexion.CerrarConexionConClose(cn);
                        return false;
                    }

                    if (diferencia <= 2)
                    {
                        string sentenciaAlerta = @"INSERT INTO AlertaStock VALUES (@IdStock, @Descripcion, @Estado);
                                                   SELECT CAST(Scope_IDentity() as int);";
                        cmd.CommandText = sentenciaAlerta;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@IdStock", stock.Id);
                        cmd.Parameters.AddWithValue("@Descripcion", "Queda poco stock");
                        cmd.Parameters.AddWithValue("@Estado", false);
                        int insert = (int)cmd.ExecuteScalar();
                    }

                    string sentenciaUpdate = @"UPDATE Stock SET cantidad = @Cantidad  WHERE idProducto = @IdProducto AND idColor = @IdColor AND idTalle = @IdTalle";

                    cmd.CommandText = sentenciaUpdate;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdProducto", stock.IdProducto);
                    cmd.Parameters.AddWithValue("@IdColor", stock.IdColor);
                    cmd.Parameters.AddWithValue("@IdTalle", stock.IdTalle);
                    cmd.Parameters.AddWithValue("@Cantidad", diferencia);

                    int affected = cmd.ExecuteNonQuery();
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

        public long ActualizarStockYCrearVenta(List<DTOStock> obj, DTOVenta dto)
        {
            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Stock WHERE idProducto = @IdProducto AND idColor = @IdColor AND idTalle = @IdTalle;";

                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;

                List<Stock> stocks = new List<Stock>();

                foreach (DTOStock item in obj)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdProducto", item.IdProducto);
                    cmd.Parameters.AddWithValue("@IdColor", item.IdColor);
                    cmd.Parameters.AddWithValue("@IdTalle", item.IdTalle);

                    Stock stock = new Stock();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stock.Id = Convert.ToInt64(reader["idStock"]);
                            stock.IdProducto = Convert.ToInt64(reader["idProducto"]);
                            stock.IdColor = Convert.ToInt64(reader["idColor"]);
                            stock.IdTalle = Convert.ToInt64(reader["idTalle"]);
                            stock.Cantidad = Convert.ToInt32(reader["cantidad"]);
                        }
                    }

                    stocks.Add(stock);
                }

                foreach (Stock stock in stocks)
                {
                    var objeto = obj.Find(o => o.IdProducto == stock.IdProducto && o.IdTalle == stock.IdTalle && o.IdColor == stock.IdColor);
                    var diferencia = stock.Cantidad - objeto.Cantidad;

                    if (diferencia < 0)
                    {
                        trn.Rollback();
                        manejadorConexion.CerrarConexionConClose(cn);
                        return -1;
                    }

                    if (diferencia <= 2)
                    {
                        string sentenciaAlerta = @"INSERT INTO AlertaStock VALUES (@IdStock, @Descripcion, @Estado);
                                                   SELECT CAST(Scope_IDentity() as int);";
                        cmd.CommandText = sentenciaAlerta;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@IdStock", stock.Id);
                        cmd.Parameters.AddWithValue("@Descripcion", "Queda poco stock");
                        cmd.Parameters.AddWithValue("@Estado", false);
                        int insert = (int)cmd.ExecuteScalar();
                    }

                    string sentenciaUpdate = @"UPDATE Stock SET cantidad = @Cantidad  WHERE idProducto = @IdProducto AND idColor = @IdColor AND idTalle = @IdTalle";

                    cmd.CommandText = sentenciaUpdate;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdProducto", stock.IdProducto);
                    cmd.Parameters.AddWithValue("@IdColor", stock.IdColor);
                    cmd.Parameters.AddWithValue("@IdTalle", stock.IdTalle);
                    cmd.Parameters.AddWithValue("@Cantidad", diferencia);

                    int affected = cmd.ExecuteNonQuery();
                }


                Venta venta = new Venta();
                venta.cargarDeDTO(dto);

                string sentenciaVenta = @"INSERT INTO Venta VALUES (@MontoTotal, @NombreComprador, @CorreoComprador, @BajaLogica, @Direccion, @Telefono, @Aprobado);
                                          SELECT CAST(Scope_IDentity() as int);";
                cmd.CommandText = sentenciaVenta;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@MontoTotal", venta.MontoTotal);
                cmd.Parameters.AddWithValue("@NombreComprador", venta.NombreComprador);
                cmd.Parameters.AddWithValue("@CorreoComprador", venta.CorreoComprador);
                cmd.Parameters.AddWithValue("@BajaLogica", false);
                cmd.Parameters.AddWithValue("@Direccion", venta.Direccion);
                cmd.Parameters.AddWithValue("@Telefono", venta.Telefono);
                cmd.Parameters.AddWithValue("@Aprobado", false);

                int idGeneradoVenta = (int)cmd.ExecuteScalar();

                cmd.CommandText = @"INSERT INTO VentaProducto VALUES (@IdVenta, @IdProducto, @IdTalle, @IdColor, @Cantidad, @Precio);
                                    SELECT CAST(Scope_IDentity() as int);";
                foreach (VentaProducto ventaProducto in venta.ProductosVendidos)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdVenta", idGeneradoVenta);
                    cmd.Parameters.AddWithValue("@IdProducto", ventaProducto.IdProducto);
                    cmd.Parameters.AddWithValue("@IdTalle", ventaProducto.IdTalle);
                    cmd.Parameters.AddWithValue("@IdColor", ventaProducto.IdColor);
                    cmd.Parameters.AddWithValue("@Cantidad", ventaProducto.Cantidad);
                    cmd.Parameters.AddWithValue("@Precio", ventaProducto.Precio);

                    cmd.ExecuteNonQuery();
                }

                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return idGeneradoVenta;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public bool Alta(DTOStock obj)
        {
            throw new NotImplementedException();
        }

        public bool BajaLogica(DTOStock obj)
        {
            throw new NotImplementedException();
        }

        public DTOStock BuscarPorId(DTOStock obj)
        {
            throw new NotImplementedException();
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

        public bool Modificar(List<DTOStock> obj)
        {
            List<Stock> stocks = new List<Stock>();
            foreach (var s in obj)
            {
                Stock stock = new Stock();
                stock.cargarDeDTO(s);
                stocks.Add(stock);
            }

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE Stock SET idProducto = @IdProducto, idColor = @IdColor, idTalle = @IdTalle, cantidad = @Cantidad  WHERE idStock = @IdStock";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;

                foreach (Stock stock in stocks)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdStock", stock.Id);
                    cmd.Parameters.AddWithValue("@IdProducto", stock.IdProducto);
                    cmd.Parameters.AddWithValue("@IdColor", stock.IdColor);
                    cmd.Parameters.AddWithValue("@IdTalle", stock.IdTalle);
                    cmd.Parameters.AddWithValue("@Cantidad", stock.Cantidad);

                    int affected = cmd.ExecuteNonQuery();
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

        public bool Modificar(DTOStock obj)
        {
            throw new NotImplementedException();
        }

        public bool TieneStock(DTOStock obj)
        {
            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Stock WHERE idProducto = @IdProducto AND idColor = @IdColor AND idTalle = @IdTalle";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdProducto", obj.IdProducto);
                cmd.Parameters.AddWithValue("@IdColor", obj.IdColor);
                cmd.Parameters.AddWithValue("@IdTalle", obj.IdTalle);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;

                Stock stock = new Stock();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stock.Id = Convert.ToInt64(reader["idStock"]);
                        stock.IdProducto = Convert.ToInt64(reader["idProducto"]);
                        stock.IdColor = Convert.ToInt64(reader["idColor"]);
                        stock.IdTalle = Convert.ToInt64(reader["idTalle"]);
                        stock.Cantidad = Convert.ToInt32(reader["cantidad"]);
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return obj.Cantidad <= stock.Cantidad;
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
                        stock.Cantidad = Convert.ToInt32(reader["cantidad"]);
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
