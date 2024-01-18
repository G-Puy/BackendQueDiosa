using Conexiones;
using Dominio.Entidades;
using DTOS;
using Repositorios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IRepositorios
{
    public class RepositorioAlertaPedido :  RepositorioBase, IRepositorioPedido
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Alta(DTOAlertaPedido obj)
        {
            throw new NotImplementedException();
        }

        public bool BajaLogica(DTOAlertaPedido obj)
        {
            throw new NotImplementedException();
        }

        public DTOAlertaPedido BuscarPorId(DTOAlertaPedido obj)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(DTOAlertaPedido obj)
        {
            throw new NotImplementedException();
        }

        public bool Entregado(long id)
        {
            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE AlertaPedido SET entregado = @Entregado WHERE idAlertaPedido = @IdAlertaPedido";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Entregado", true);
                cmd.Parameters.AddWithValue("@IdAlertaPedido", id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                cmd.ExecuteNonQuery();
                trn.Rollback();
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

        public bool Modificar(DTOAlertaPedido obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DTOAlertaPedido> TraerTodos()
        {
            List<DTOAlertaPedido> dtos = new List<DTOAlertaPedido>();
            List<AlertaPedido> alertas = new List<AlertaPedido>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM AlertaPedido";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AlertaPedido alerta = new AlertaPedido();
                        alerta.Id = Convert.ToInt64(reader["idAlertaPedido"]);
                        alerta.IdVenta = Convert.ToInt64(reader["idVenta"]);
                        alerta.Descripcion = Convert.ToString(reader["descripcion"]);
                        alerta.Entregado = Convert.ToBoolean(reader["entregado"]);
                        alertas.Add(alerta);
                    }
                }

                var secuenciaVenta = @"SELECT * FROM Venta WHERE idVenta = @IdVenta";
                var secuenciaVentaProducto = @"SELECT * FROM VentaProducto WHERE idVenta = @IdVenta";
                foreach (AlertaPedido alerta in alertas)
                {
                    DTOAlertaPedido dto = alerta.darDto();

                    cmd.CommandText = secuenciaVenta;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdVenta", alerta.IdVenta);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Venta venta = new Venta();
                            venta.IdVenta = Convert.ToInt64(reader["idVenta"]);
                            venta.MontoTotal = Convert.ToDecimal(reader["montoTotal"]);
                            venta.NombreComprador = Convert.ToString(reader["nombreComprador"]);
                            venta.CorreoComprador = Convert.ToString(reader["correoComprador"]);
                            venta.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                            venta.Direccion = Convert.ToString(reader["direccion"]);
                            venta.Telefono = Convert.ToString(reader["telefono"]);
                            venta.Aprobado = Convert.ToBoolean(reader["aprobado"]);
                            venta.ApellidoComprador = Convert.ToString(reader["apellidoComprador"]);
                            venta.Envio = Convert.ToBoolean(reader["envio"]);
                            dto.Venta = venta.darDto();
                        }
                    }

                    cmd.CommandText = secuenciaVentaProducto;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VentaProducto vp = new VentaProducto();
                            vp.IdVenta = Convert.ToInt64(reader["idVenta"]);
                            vp.IdProducto = Convert.ToInt64(reader["idProducto"]);
                            vp.IdTalle = Convert.ToInt64(reader["idTalle"]);
                            vp.IdColor = Convert.ToInt64(reader["idColor"]);
                            vp.Cantidad = Convert.ToInt32(reader["cantidad"]);
                            vp.Precio = Convert.ToDecimal(reader["precio"]);
                            DTOVentaProducto dtoTipoT = vp.darDto();
                            dto.Venta.ProductosVendidos.Add(dtoTipoT);
                        }
                    }


                    dtos.Add(dto);
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                return dtos;
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
