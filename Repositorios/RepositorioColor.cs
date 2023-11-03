using Conexiones;
using Dominio.Entidades;
using DTOS;
using IRepositorios;
using System.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Repositorios
{
    public class RepositorioColor : RepositorioBase, IRepositorioColor
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Alta(DTOColor obj)
        {
           Color color = new Color();
            color.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"INSERT INTO Color VALUES(@NombreColor,@BajaLogica)
                                    SELECT CAST(Scope_IDentity() as int)";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@NombreColor",color.Nombre);
                cmd.Parameters.AddWithValue("@BajaLogica", color.BajaLogica);
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

        public bool BajaLogica(DTOColor obj)
        {
            Color color = new Color();
            color.cargarDeDTO(obj);


            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE TABLE Color SET bajaLogica = @BajaLogica WHERE idColor = @idColor";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idColor", color.IdColor);
                cmd.Parameters.AddWithValue("@BajaLogica", color.BajaLogica);
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

        public DTOColor BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(DTOColor obj)
        {
            Color color = new Color();
            color.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"DELETE FROM Color WHERE idColor = @idColor";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idColor", color.IdColor);
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

        public bool Modificar(DTOColor obj)
        {
            Color color = new Color();
            color.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE Color SET nombre = @nombre WHERE idColor = @idColor";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idColor", color.IdColor);
                cmd.Parameters.AddWithValue("@Nombre", color.Nombre);
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

        public IEnumerable<DTOColor> TraerTodos()
        {
            List<DTOColor> tipos = new List<DTOColor>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Color";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Color color = new Color();

                        color.IdColor = reader.GetInt32(0);
                        color.Nombre = reader.GetString(1);
                        color.BajaLogica = reader.GetBoolean(2);
                        DTOColor dtoTipoC = color.darDto();

                        tipos.Add(dtoTipoC);
                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                return (IEnumerable<DTOColor>)tipos;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public bool VerificarExistenciaColor(DTOColor DTOCol)
        {
            Color color = new Color();
            color.cargarDeDTO(DTOCol);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {

                //TODO: para dar de alta un tipo de producto no usamos id, por lo tanto no se puede confirmar si exsite por id
                //Hay que confirmarlo haciendo uppercase en el parametro de entrada, y upper case en el parametro de la bd
                //para asi comparar por nombre los 2 en mayuscula
                string sentenciaSql = @"SELECT * FROM Color WHERE idColor = @idColor";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idTipoProducto", color.IdColor);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        color.IdColor = reader.GetInt32(0);
                        color.Nombre = reader.GetString(1);
                        color.BajaLogica = reader.GetBoolean(2);
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

        IEnumerable<DTOColor> IRepositorioT<DTOColor>.TraerTodos()
        {
            throw new NotImplementedException();
        }
    }
}

