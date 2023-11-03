using Conexiones;
using Dominio.Entidades;
using DTOS;
using IRepositorios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios
{
    public class RepositorioTipoPrenda : RepositorioBase, IRepositorioTipoPrenda
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Alta(DTOTipoPrenda obj)
        {
            TipoPrenda tipoPrenda = new TipoPrenda();
            tipoPrenda.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"INSERT INTO TipoProducto VALUES(@NombreTipoPrenda,@BajaLogica)
                                    SELECT CAST(Scope_IDentity() as int)";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@NombreTipoPrenda", tipoPrenda.NombreTipoPrenda);
                cmd.Parameters.AddWithValue("@BajaLogica", tipoPrenda.BajaLogica);
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

        public bool BajaLogica(DTOTipoPrenda obj)
        {
            TipoPrenda tipoPrenda = new TipoPrenda();
            tipoPrenda.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE TABLE TipoProducto SET bajaLogica = @BajaLogica WHERE idTipoProducto = @idTipoProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idTipoProducto", tipoPrenda.IdTipoPrenda);
                cmd.Parameters.AddWithValue("@BajaLogica", tipoPrenda.BajaLogica);
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

        public DTOTipoPrenda BuscarPorId(int id)
        {
            TipoPrenda tipoPrenda = new TipoPrenda();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM TipoProducto WHERE idTipoProducto = @idTipoProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idTipoProducto", id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tipoPrenda.IdTipoPrenda = reader.GetInt32(0);
                        tipoPrenda.NombreTipoPrenda = reader.GetString(1);
                        tipoPrenda.BajaLogica = reader.GetBoolean(2);
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return tipoPrenda.darDto();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public bool Eliminar(DTOTipoPrenda obj)
        {
            TipoPrenda tipoPrenda = new TipoPrenda();
            tipoPrenda.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"DELETE FROM TipoProducto WHERE idTipoProducto = @idTipoProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idTipoProducto", tipoPrenda.IdTipoPrenda);
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

        public bool Modificar(DTOTipoPrenda obj)
        {
            TipoPrenda tipoPrenda = new TipoPrenda();
            tipoPrenda.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE TABLE TipoProducto SET nombre = @nombre, bajaLogica = @BajaLogica WHERE idTipoProducto = @idTipoProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idTipoProducto", tipoPrenda.IdTipoPrenda);
                cmd.Parameters.AddWithValue("@Nombre", tipoPrenda.NombreTipoPrenda);
                cmd.Parameters.AddWithValue("@BajaLogica", tipoPrenda.BajaLogica);
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

        public IEnumerable<DTOTipoPrenda> TraerTodos()
        {
            List<TipoPrenda> tipos = new List<TipoPrenda>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM TipoProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TipoPrenda tipoPrenda = new TipoPrenda();

                        tipoPrenda.IdTipoPrenda = reader.GetInt32(0);
                        tipoPrenda.NombreTipoPrenda = reader.GetString(1);
                        tipoPrenda.BajaLogica = reader.GetBoolean(2);

                        tipos.Add(tipoPrenda);
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return (IEnumerable<DTOTipoPrenda>)tipos;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public bool VerificarExistenciaCategoria(DTOTipoPrenda DTOCategoria)
        {
            TipoPrenda tipoPrenda = new TipoPrenda();
            tipoPrenda.cargarDeDTO(DTOCategoria);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {

               //TODO: para dar de alta un tipo de producto no usamos id, por lo tanto no se puede confirmar si exsite por id
               //Hay que confirmarlo haciendo uppercase en el parametro de entrada, y upper case en el parametro de la bd
               //para asi comparar por nombre los 2 en mayuscula
                string sentenciaSql = @"SELECT * FROM TipoProducto WHERE idTipoProducto = @idTipoProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idTipoProducto", tipoPrenda.IdTipoPrenda);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tipoPrenda.IdTipoPrenda = reader.GetInt32(0);
                        tipoPrenda.NombreTipoPrenda = reader.GetString(1);
                        tipoPrenda.BajaLogica = reader.GetBoolean(2);
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
    }
}
