﻿using Conexiones;
using Dominio.Entidades;
using DTOS;
using DTOS.DTOSProductoFrontBack;
using IRepositorios;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace Repositorios
{
    public class RepositorioStock : RepositorioBase, IRepositorioStock
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

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
                        cmd.CommandText = @"SELECT nombre FROM Producto WHERE idProducto = @IdProducto";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@IdProducto", stock.IdProducto);
                        string nombreProducto = "";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nombreProducto = Convert.ToString(reader["nombre"]);
                            }
                        }

                        cmd.CommandText = @"SELECT nombre FROM Talle WHERE idTalle = @IdTalle";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@IdTalle", stock.IdTalle);
                        string nombreTalle = "";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nombreTalle = Convert.ToString(reader["nombre"]);
                            }
                        }

                        cmd.CommandText = @"SELECT nombre FROM Color WHERE idColor = @IdColor";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@IdColor", stock.IdColor);
                        string nombreColor = "";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nombreColor = Convert.ToString(reader["nombre"]);
                            }
                        }


                        string sentenciaAlerta = @"INSERT INTO AlertaStock VALUES (@Leida, @NombreProducto, @NombreTalle, @NombreColor, @Cantidad, @IdProducto, @Fecha);
                                                   SELECT CAST(Scope_IDentity() as int);";
                        cmd.CommandText = sentenciaAlerta;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@IdStock", stock.Id);
                        cmd.Parameters.AddWithValue("@Leida", false);
                        cmd.Parameters.AddWithValue("@NombreProducto", nombreProducto);
                        cmd.Parameters.AddWithValue("@NombreTalle", nombreTalle);
                        cmd.Parameters.AddWithValue("@NombreColor", nombreColor);
                        cmd.Parameters.AddWithValue("@Cantidad", diferencia);
                        cmd.Parameters.AddWithValue("@IdProducto", stock.IdProducto);
                        cmd.Parameters.AddWithValue("@Fecha", DateTime.Now);
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

                string sentenciaVenta = @"INSERT INTO Venta VALUES(@MontoTotal,@NombreComprador, @CorreoComprador, @BajaLogica, @Direccion, @Telefono, @Aprobado, @ApellidoComprador, @Envio, @Fecha, @Notas)
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
                cmd.Parameters.AddWithValue("@ApellidoComprador", venta.ApellidoComprador);
                cmd.Parameters.AddWithValue("@Envio", venta.Envio);
                cmd.Parameters.AddWithValue("@Fecha", venta.Fecha);
                cmd.Parameters.AddWithValue("@Notas", venta.Notas);

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
                string sentenciaStock = @"DELETE FROM Stock WHERE idStock = @IdStock";

                SqlCommand cmd = new SqlCommand(sentenciaStock, cn);
                cmd.Parameters.AddWithValue("@IdStock", stock.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int affected = cmd.ExecuteNonQuery();

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
