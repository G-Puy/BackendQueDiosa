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
                string sentenciaSql = @"INSERT INTO Color VALUES(@Nombre,@BajaLogica)
                                    SELECT CAST(Scope_IDentity() as int)";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Nombre",color.Nombre);
                cmd.Parameters.AddWithValue("@BajaLogica", false);
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
                string sentenciaSql = @"UPDATE Color SET bajaLogica = @BajaLogica WHERE idColor = @idColor";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idColor", color.Id);
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

        public DTOColor BuscarPorId(DTOColor dtoColor)
        {
            Color color = new Color();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Color WHERE idColor = @IdColor";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdColor", dtoColor.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        color.Id = Convert.ToInt64(reader["idColor"]);
                        color.Nombre = Convert.ToString(reader["nombre"]);
                        color.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);

                if (color == null || color.Id == 0)
                {
                    return null;
                }

                return color.darDto();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public DTOColor BuscarPorNombre(DTOColor dtoColor)
        {
            Color color = new Color();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Color WHERE UPPER(nombre) = @Nombre";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Nombre", dtoColor.Nombre.ToUpper());
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        color.Id = Convert.ToInt64(reader["idColor"]);
                        color.Nombre = Convert.ToString(reader["nombre"]);
                        color.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);

                if (color == null || color.Id == 0)
                {
                    return null;
                }

                return color.darDto();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
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
                cmd.Parameters.AddWithValue("@idColor", color.Id);
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

        public bool EnUso(DTOColor dtoColor)
        {
            Color color = new Color();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT TOP 1 idColor FROM Stock WHERE idColor = @IdColor";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdColor", dtoColor.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        color.Id = Convert.ToInt64(reader["idColor"]);
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return color != null && color.Id > 0;
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
                string sentenciaSql = @"UPDATE Color SET nombre = @Nombre WHERE idColor = @IdColor";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdColor", color.Id);
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
            List<DTOColor> colores = new List<DTOColor>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Color WHERE bajaLogica = 0";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Color color = new Color();

                        color.Id = Convert.ToInt64(reader["idColor"]);
                        color.Nombre = Convert.ToString(reader["nombre"]);
                        color.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                        DTOColor dtoColor = color.darDto();

                        colores.Add(dtoColor);
                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                return (IEnumerable<DTOColor>)colores;
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

