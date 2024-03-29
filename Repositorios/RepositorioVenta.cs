﻿using Conexiones;
using Dominio.Entidades;
using DTOS;
using DTOS.DTOSProductoFrontBack;
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
                string sentenciaSql = @"INSERT INTO Venta VALUES(@MontoTotal,@NombreComprador, @CorreoComprador, @BajaLogica, @Direccion, @Telefono, @Aprobado, @ApellidoComprador, @Envio, @Fecha, @Notas);
                                        SELECT CAST(Scope_IDentity() as int);";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@MontoTotal", venta.MontoTotal);
                cmd.Parameters.AddWithValue("@NombreComprador", venta.NombreComprador);
                cmd.Parameters.AddWithValue("@CorreoComprador", venta.CorreoComprador);
                cmd.Parameters.AddWithValue("@BajaLogica", false);
                cmd.Parameters.AddWithValue("@Direccion", venta.Direccion);
                cmd.Parameters.AddWithValue("@Telefono", venta.Telefono);
                cmd.Parameters.AddWithValue("@Aprobado", venta.Aprobado);
                cmd.Parameters.AddWithValue("@ApellidoComprador", venta.ApellidoComprador);
                cmd.Parameters.AddWithValue("@Envio", venta.Envio);
                cmd.Parameters.AddWithValue("@Fecha", venta.Fecha);
                cmd.Parameters.AddWithValue("@Notas", venta.Notas);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int idGenerado = (int)cmd.ExecuteScalar();

                cmd.CommandText = @"INSERT INTO VentaProducto VALUES(@IdVenta, @IdProducto, @IdTalle, @IdColor, @Cantidad, @Precio);
                                  SELECT CAST(Scope_IDentity() as int);";
                foreach (VentaProducto ventaProducto in venta.ProductosVendidos)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdVenta", idGenerado);
                    cmd.Parameters.AddWithValue("@IdProducto", ventaProducto.IdProducto);
                    cmd.Parameters.AddWithValue("@IdTalle", ventaProducto.IdTalle);
                    cmd.Parameters.AddWithValue("@IdColor", ventaProducto.IdColor);
                    cmd.Parameters.AddWithValue("@Cantidad", ventaProducto.Cantidad);
                    cmd.Parameters.AddWithValue("@Precio", ventaProducto.Precio);

                    cmd.ExecuteScalar();
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
            List<DTOVenta> ventas = new List<DTOVenta>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Venta WHERE bajaLogica = 0";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
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
                        venta.Fecha = Convert.ToDateTime(reader["fecha"]);
                        venta.Notas = Convert.ToString(reader["Notas"]);
                        DTOVenta dtoTipoT = venta.darDto();

                        ventas.Add(dtoTipoT);
                    }
                }

                cmd.CommandText = "SELECT * FROM VentaProducto WHERE idVenta = @IdVenta";
                foreach (DTOVenta venta in ventas)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdVenta", venta.IdVenta);

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

                            venta.ProductosVendidos.Add(dtoTipoT);
                        }
                    }
                }

                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                return (IEnumerable<DTOVenta>)ventas;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
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

                cmd.CommandText = @"INSERT INTO AlertaPedido VALUES (@IdVenta, @Realizado);
                                    SELECT CAST(Scope_IDentity() as int);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IdVenta", idVenta);
                //cmd.Parameters.AddWithValue("@Descripcion", "descripcion");
                cmd.Parameters.AddWithValue("@Realizado", false);
                cmd.ExecuteScalar();

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

        public List<DTODetallePedido> TraerDetallePedido(long idVenta)
        {
            List<VentaProducto> vps = new List<VentaProducto>();
            List<DTODetallePedido> dps = new List<DTODetallePedido>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = "SELECT * FROM VentaProducto WHERE idVenta = @IdVenta";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                cmd.Parameters.AddWithValue("@IdVenta", idVenta);

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
                        vps.Add(vp);
                    }
                }

                foreach (var v in vps)
                {
                    var nombreProducto = "";
                    var nombreTalle = "";
                    var nombreColor = "";

                    cmd.CommandText = @"SELECT nombre FROM Producto WHERE idProducto = @IdProducto;";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdProducto", v.IdProducto);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nombreProducto = Convert.ToString(reader["nombre"]);
                        }
                    }

                    cmd.CommandText = @"SELECT nombre FROM Talle WHERE idTalle = @IdTalle;";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdTalle", v.IdTalle);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nombreTalle = Convert.ToString(reader["nombre"]);
                        }
                    }

                    cmd.CommandText = @"SELECT nombre FROM Color WHERE idColor= @IdColor;";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdColor", v.IdColor);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nombreColor = Convert.ToString(reader["nombre"]);
                        }
                    }

                    DTODetallePedido dp = new DTODetallePedido();
                    dp.Cantidad = v.Cantidad;
                    dp.IdProducto = v.IdProducto;
                    dp.NombreProducto = nombreProducto;
                    dp.NombreTalle = nombreTalle;
                    dp.NombreColor = nombreColor;
                    dps.Add(dp);
                }

                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                return dps;
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
