using Conexiones;
using Dominio.Entidades;
using DTOS;
using IRepositorios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios
{
    public class RepositorioTalle : RepositorioBase, IRepositorioTalle
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Alta(DTOTalle obj)
        {
            Talle talle = new Talle();
            talle.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"INSERT INTO Talle VALUES(@Nombre,@BajaLogica)
                                    SELECT CAST(Scope_IDentity() as int)";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Nombre", talle.Nombre);
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

        public bool BajaLogica(DTOTalle obj)
        {
            Talle talle = new Talle();
            talle.cargarDeDTO(obj);


            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE Talle SET bajaLogica = @BajaLogica WHERE idTalle = @idTalle;";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idTalle", talle.Id);
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

        public DTOTalle BuscarPorId(DTOTalle dtoTalle)
        {
            Talle talle = new Talle();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Talle WHERE idTalle = @idTalle";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idTalle", dtoTalle.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        talle.Id = Convert.ToInt64(reader["idTalle"]);
                        talle.Nombre = Convert.ToString(reader["nombre"]);
                        talle.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);

                if (talle == null || talle.Id == 0)
                {
                    return null;
                }

                return talle.darDto();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public DTOTalle BuscarPorNombre(DTOTalle dtoTalle)
        {
            Talle talle = new Talle();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Talle WHERE UPPER(nombre) = @Nombre";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Nombre", dtoTalle.Nombre.ToUpper());
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        talle.Id = Convert.ToInt64(reader["idTalle"]);
                        talle.Nombre = Convert.ToString(reader["nombre"]);
                        talle.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);

                if (talle == null || talle.Id == 0)
                {
                    return null;
                }

                return talle.darDto();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public bool Eliminar(DTOTalle obj)
        {
            Talle talle = new Talle();
            talle.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"DELETE FROM Talle WHERE idTalle = @idTalle";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idTalle", talle.Id);
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

        public bool EnUso(DTOTalle dtoTalle)
        {
            Talle talle = new Talle();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT TOP 1 idTalle FROM Stock WHERE idTalle = @IdTalle;";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdTalle", dtoTalle.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        talle.Id = Convert.ToInt64(reader["idTalle"]);
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return talle != null && talle.Id > 0;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public bool Modificar(DTOTalle obj)
        {
            Talle talle = new Talle();
            talle.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE Talle SET nombre = @Nombre WHERE idTalle = @idTalle";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idTalle", talle.Id);
                cmd.Parameters.AddWithValue("@Nombre", talle.Nombre);
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

        public bool NombreOcupado(DTOTalle dto)
        {
            List<Talle> talles = new List<Talle>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Talle WHERE UPPER(nombre) = @Nombre AND idTalle != @IdTalle;";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Nombre", dto.Nombre.ToUpper());
                cmd.Parameters.AddWithValue("@IdTalle", dto.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Talle t = new Talle();
                        t.Id = Convert.ToInt64(reader["idTalle"]);
                        t.Nombre = Convert.ToString(reader["nombre"]);
                        t.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                        talles.Add(t);
                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);

                return talles.Count() > 0;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public IEnumerable<DTOTalle> TraerTodos()
        {
            List<DTOTalle> talles = new List<DTOTalle>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Talle WHERE bajaLogica = 0";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Talle talle = new Talle();

                        talle.Id = Convert.ToInt64(reader["idTalle"]);
                        talle.Nombre = Convert.ToString(reader["nombre"]);
                        talle.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                        DTOTalle dtoTipoT = talle.darDto();

                        talles.Add(dtoTipoT);
                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                return (IEnumerable<DTOTalle>)talles;
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
