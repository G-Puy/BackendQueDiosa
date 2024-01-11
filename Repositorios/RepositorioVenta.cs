using Conexiones;
using Dominio.Entidades;
using DTOS;
using IRepositorios;
using Serilog;
using System;
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
                string sentenciaSql = @"INSERT INTO Venta VALUES(@MontoTotal,@NombreComprador, @CorreoComprador, @BajaLogica, @Direccion, @Telefono, @IdPreferencia)
                                        SELECT CAST(Scope_IDentity() as int)";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@MontoTotal", venta.MontoTotal); 
                cmd.Parameters.AddWithValue("@NombreComprador", venta.NombreComprador);
                cmd.Parameters.AddWithValue("@CorreoComprador", venta.CorreoComprador);
                cmd.Parameters.AddWithValue("@BajaLogica", false);
                cmd.Parameters.AddWithValue("@Direccion", venta.Direccion);
                cmd.Parameters.AddWithValue("@Telefono", venta.Telefono);
                cmd.Parameters.AddWithValue("@IdPreferencia", venta.IdPreferencia);
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
    }
}
