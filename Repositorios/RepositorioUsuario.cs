﻿using Conexiones;
using Dominio.Entidades;
using DTOS;
using IRepositorios;
using System.Data.SqlClient;

namespace Repositorios
{
    public class RepositorioUsuario : RepositorioBase, IRepositorioUsuario
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
                cmd.Parameters.AddWithValue("@Contrasenia", usuario.Contrasenia);
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

        public DTOUsuario BuscarPorId(DTOUsuario dtoUsuario)
        {
            Usuario usuario = new Usuario();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT TOP 1 * FROM Usuario WHERE idUsuario = @IdUsuario";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdUsuario", dtoUsuario.IdUsuario);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuario.IdUsuario = (long)reader["idUsuario"];
                        usuario.NombreDeUsuario = reader["nombreDeUsuario"].ToString();
                        usuario.Contrasenia = reader["contrasenia"].ToString();
                        usuario.Nombre = reader["nombre"].ToString();
                        usuario.Apellido = reader["apellido"].ToString();
                        usuario.Telefono = reader["telefono"].ToString();
                        usuario.Correo = reader["correo"].ToString();
                        usuario.BajaLogica = (bool)reader["bajaLogica"];
                        usuario.IdTipoUsuario = (long)reader["idTipoUsuario"];

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

        public DTOUsuario BuscarPorNombreDeUsuario(DTOUsuario dtoUsuario)
        {
            Usuario usuario = new Usuario();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT TOP 1 * FROM Usuario WHERE nombreDeUsuario = @NombreDeUsuario";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@NombreDeUsuario", usuario.NombreDeUsuario);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuario.IdUsuario = (long)reader["idUsuario"];
                        usuario.NombreDeUsuario = reader["nombreDeUsuario"].ToString();
                        usuario.Contrasenia = reader["contrasenia"].ToString();
                        usuario.Nombre = reader["nombre"].ToString();
                        usuario.Apellido = reader["apellido"].ToString();
                        usuario.Telefono = reader["telefono"].ToString();
                        usuario.Correo = reader["correo"].ToString();
                        usuario.BajaLogica = (bool)reader["bajaLogica"];
                        usuario.IdTipoUsuario = (long)reader["idTipoUsuario"];

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

        public DTOUsuario Login(DTOUsuario dtoUsuario)
        {
            Usuario usuario = new Usuario();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Usuario WHERE nombreDeUsuario = @NombreDeUsuario AND contrasenia = @Contrasenia";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@NombreDeUsuario", usuario.NombreDeUsuario);
                cmd.Parameters.AddWithValue("@Contrasenia", usuario.Contrasenia);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuario.NombreDeUsuario = reader["nombreDeUsuario"].ToString();
                        usuario.Contrasenia = reader["contrasenia"].ToString();
                        usuario.Nombre = reader["nombre"].ToString();
                        usuario.Apellido = reader["apellido"].ToString();
                        usuario.IdTipoUsuario = Convert.ToInt64(reader["idTipoUsuario"]);

                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);

                if (usuario == null || usuario.IdUsuario == 0)
                {
                    return null;
                }

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
                cmd.Parameters.AddWithValue("@Contrasenia", usuario.Contrasenia);
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

                        usuario.IdUsuario = (long)reader["idUsuario"];
                        usuario.NombreDeUsuario = reader["nombreDeUsuario"].ToString();
                        usuario.Contrasenia = reader["contrasenia"].ToString();
                        usuario.Nombre = reader["nombre"].ToString();
                        usuario.Apellido = reader["apellido"].ToString();
                        usuario.Telefono = reader["telefono"].ToString();
                        usuario.Correo = reader["correo"].ToString();
                        usuario.BajaLogica = (bool)reader["bajaLogica"];
                        usuario.IdTipoUsuario = (long)reader["idTipoUsuario"];

                        DTOUsuario dtoTipoP = usuario.darDto();

                        tipos.Add(dtoTipoP);
                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                return tipos;
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
