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
    public class RepositorioUsuario : RepositorioBase, IRepositorioLogin
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Alta(DTOUsuario obj)
        {
            Usuario usuario = new Usuario();
            usuario.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"INSERT INTO Usuario VALUES(@NombreUsuario, @Contrasenia, @Nombre, @Apellido, @Telefono, @Correo, @BajaLogica, @IdTipoUsuario)
                                    SELECT CAST(Scope_IDentity() as int)";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreDeUsuario);
                cmd.Parameters.AddWithValue("@Contrasenia", usuario.Contraseña);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("@BajaLogica", usuario.BajaLogica);
                cmd.Parameters.AddWithValue("@IdTipoUsuario", usuario.IdTipoUsuario);
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

        public bool BajaLogica(DTOUsuario obj)
        {
            Usuario usuario = new Usuario();
            usuario.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE TABLE Usuario SET bajaLogica = @BajaLogica WHERE idUsuario = @IdUsuario";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                cmd.Parameters.AddWithValue("@BajaLogica", usuario.BajaLogica);
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

        public DTOUsuario BuscarPorId(int id)
        {
            Usuario usuario = new Usuario();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT TOP 1 * FROM Usuario WHERE idUsuario = @IdUsuario";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdUsuario", id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuario.IdUsuario = reader.GetInt32(0);
                        usuario.NombreDeUsuario = reader.GetString(1);
                        usuario.Contraseña = reader.GetString(2);
                        usuario.Nombre = reader.GetString(3);
                        usuario.Apellido = reader.GetString(4);
                        usuario.Telefono = reader.GetString(5);
                        usuario.Correo = reader.GetString(6);
                        usuario.BajaLogica = reader.GetBoolean(7);
                        usuario.IdTipoUsuario = reader.GetInt32(8);

                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return usuario.darDto();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public bool Eliminar(DTOUsuario obj)
        {
            Usuario usuario = new Usuario();
            usuario.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"DELETE FROM TipoProducto WHERE idUsuario = @IdUsuario";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
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

        public bool Login(DTOUsuario dtoUsuario)
        {
            Usuario usuario = new Usuario();
            usuario.cargarDeDTO(dtoUsuario);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Usuario WHERE nombreDeUsuario = @NombreDeUsuario AND contrasenia = @Contrasenia";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@NombreDeUsuario", usuario.NombreDeUsuario);
                cmd.Parameters.AddWithValue("@Contrasenia", usuario.Contraseña);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuario.NombreDeUsuario = reader["nombre"].ToString();
                        usuario.Contraseña = reader["contrasenia"].ToString();
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

        public bool Modificar(DTOUsuario obj)
        {
            Usuario usuario = new Usuario();
            usuario.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE Usuario SET nombreDeUsuario = @NombreDeUsuario, contrasenia = @Contrasenia, nombre = @Nombre, apellido = @Apellido, telefono = @Telefono, correo = @Correo, bajaLogica = @BajaLogica, idTipoUsuario = @IdTipoUsuario WHERE idUsuario = @IdUsuario";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreDeUsuario);
                cmd.Parameters.AddWithValue("@Contrasenia", usuario.Contraseña);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("@BajaLogica", usuario.BajaLogica);
                cmd.Parameters.AddWithValue("@IdTipoUsuario", usuario.IdTipoUsuario);
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

        public IEnumerable<DTOUsuario> TraerTodos()
        {
            List<DTOUsuario> tipos = new List<DTOUsuario>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Usuario";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Usuario usuario = new Usuario();

                        usuario.IdUsuario = reader.GetInt32(0);
                        usuario.NombreDeUsuario = reader.GetString(1);
                        usuario.Contraseña = reader.GetString(2);
                        usuario.Nombre = reader.GetString(3);
                        usuario.Apellido = reader.GetString(4);
                        usuario.Telefono = reader.GetString(5);
                        usuario.Correo = reader.GetString(6);
                        usuario.BajaLogica = reader.GetBoolean(7);
                        usuario.IdTipoUsuario = reader.GetInt32(8);
                        DTOUsuario dtoTipoP = usuario.darDto();

                        tipos.Add(dtoTipoP);
                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                return (IEnumerable<DTOUsuario>)tipos;
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
