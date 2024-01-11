using Conexiones;
using Dominio.Entidades;
using DTOS;
using IRepositorios;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios
{
    public class RepositorioVenta : RepositorioBase, IRepositorioVenta
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Alta(DTOVenta obj)
        {
            Venta venta = new Venta();
            venta.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"INSERT INTO Venta VALUES(@MontoTotal,@NombreComprador, @CorreoComprador, @BajaLogica, @Direccion, @Telefono)
                                        SELECT CAST(Scope_IDentity() as int)";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@MontoTotal", venta.MontoTotal);
                cmd.Parameters.AddWithValue("@NombreComprador", venta.NombreComprador);
                cmd.Parameters.AddWithValue("@CorreoComprador", venta.CorreoComprador);
                cmd.Parameters.AddWithValue("@BajaLogica", false);
                cmd.Parameters.AddWithValue("@Direccion", venta.Direccion);
                cmd.Parameters.AddWithValue("@Telefono", venta.Telefono);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int idGenerado = (int)cmd.ExecuteScalar();

                cmd.CommandText = @"INSERT INTO VentaProducto VALUES(@IdVenta, @IdProducto, @IdTalle, @IdColor, @Cantidad, @Precio)
                                  SELECT CAST(Scope_IDentity() as int)";
                foreach (VentaProducto ventaProducto in venta.ProductosVendidos)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdVenta", ventaProducto.IdVenta);
                    cmd.Parameters.AddWithValue("@IdProducto", ventaProducto.IdProducto);
                    cmd.Parameters.AddWithValue("@IdTalle", ventaProducto.IdTalle);
                    cmd.Parameters.AddWithValue("@IdColor", ventaProducto.IdColor);
                    cmd.Parameters.AddWithValue("@Cantidad", ventaProducto.Cantidad);
                    cmd.Parameters.AddWithValue("@Precio", ventaProducto.Precio);

                    idGenerado = (int)cmd.ExecuteScalar();
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

        public bool BajaLogica(DTOVenta obj)
        {
            throw new NotImplementedException();
        }

        public DTOVenta BuscarPorId(DTOVenta obj)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(DTOVenta obj)
        {
            throw new NotImplementedException();
        }

        public bool Modificar(DTOVenta obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DTOVenta> TraerTodos()
        {
            throw new NotImplementedException();
        }

        public bool Confirmar(long idVenta)
        {
            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE Venta SET aprobado = @Aprobado WHERE idVenta = @IdVenta;";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdVenta", idVenta);
                cmd.Parameters.AddWithValue("@Aprobado", true);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                cmd.ExecuteNonQuery();

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

        public bool Cancelar(long idVenta)
        {
            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSelect = @"SELECT * FROM VentaProducto WHERE idVenta = @IdVenta;";
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                SqlCommand cmd = new SqlCommand(sentenciaSelect, cn);
                cmd.Transaction = trn;
                cmd.Parameters.AddWithValue("@IdVenta", idVenta);

                List<VentaProducto> list = new List<VentaProducto>();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        VentaProducto v = new VentaProducto();
                        v.IdVenta = Convert.ToInt64(reader["idVenta"]);
                        v.IdProducto = Convert.ToInt64(reader["idProducto"]);
                        v.IdColor = Convert.ToInt64(reader["idColor"]);
                        v.IdTalle = Convert.ToInt64(reader["idTalle"]);
                        v.Cantidad = Convert.ToInt32(reader["cantidad"]);
                        v.Precio = Convert.ToDecimal(reader["Precio"]);
                        list.Add(v);
                    }
                }

                string sentenciaSelectStock = @"SELECT * FROM Stock WHERE idProducto = @IdProducto AND idColor = @IdColor AND idTalle = @IdTalle";
                string sentenciaUpdateStock = @"UPDATE Stock SET cantidad = @Cantidad WHERE idStock = @IdStock;";

                foreach (VentaProducto v in list)
                {
                    cmd.CommandText = sentenciaSelectStock;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdProducto", v.IdProducto);
                    cmd.Parameters.AddWithValue("@IdColor", v.IdColor);
                    cmd.Parameters.AddWithValue("@IdTalle", v.IdTalle);

                    DTOStock stock = new DTOStock();
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

                    int cantidadNueva = stock.Cantidad + v.Cantidad;

                    cmd.CommandText = sentenciaUpdateStock;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdStock", stock.Id);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidadNueva);
                    cmd.ExecuteNonQuery();


                    string sentenciaDeleteVP = @"DELETE FROM VentaProducto WHERE idVenta = @IdVenta;
                                                 DELETE FROM Venta WHERE idVenta = @IdVenta;";
                    cmd.CommandText = sentenciaDeleteVP;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdVenta", v.IdVenta);
                    cmd.ExecuteNonQuery();
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
    }
}
