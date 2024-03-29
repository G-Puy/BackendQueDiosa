﻿using Conexiones;
using Dominio.Entidades;
using DTOS;
using IRepositorios;
using System.Data.SqlClient;

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
                string sentenciaSql = @"INSERT INTO TipoProducto VALUES(@Nombre,@BajaLogica)
                                    SELECT CAST(Scope_IDentity() as int)";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Nombre", tipoPrenda.Nombre);
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

        public bool BajaLogica(DTOTipoPrenda obj)
        {
            TipoPrenda tipoPrenda = new TipoPrenda();
            tipoPrenda.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE TipoProducto SET bajaLogica = @BajaLogica WHERE idTipoProducto = @IdTipoProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdTipoProducto", tipoPrenda.Id);
                cmd.Parameters.AddWithValue("@BajaLogica", true);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int rowsAffected = cmd.ExecuteNonQuery();
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public DTOTipoPrenda BuscarPorId(DTOTipoPrenda dtoTipoPrenda)
        {
            TipoPrenda tipoPrenda = new TipoPrenda();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM TipoProducto WHERE idTipoProducto = @idTipoProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idTipoProducto", dtoTipoPrenda.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tipoPrenda.Id = Convert.ToInt64(reader["idTipoProducto"]);
                        tipoPrenda.Nombre = reader["nombre"].ToString();
                        tipoPrenda.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);

                if (tipoPrenda == null || tipoPrenda.Id == 0)
                {
                    return null;
                }

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

        public DTOTipoPrenda BuscarPorNombre(DTOTipoPrenda dtoTipoPrenda)
        {
            TipoPrenda prenda = new TipoPrenda();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM TipoProducto WHERE UPPER(nombre) = @Nombre";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Nombre", dtoTipoPrenda.Nombre.ToUpper());
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        prenda.Id = Convert.ToInt64(reader["idTipoProducto"]);
                        prenda.Nombre = reader["nombre"].ToString();
                        prenda.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);

                if (prenda == null || prenda.Id == 0)
                {
                    return null;
                }

                return prenda.darDto();
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
                string sentenciaSql = @"DELETE FROM TipoProducto WHERE idTipoProducto = @IdTipoProducto;";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdTipoProducto", tipoPrenda.Id);
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

        public bool EnUso(DTOTipoPrenda dtoTipoPrenda)
        {
            TipoPrenda tipoPrenda = new TipoPrenda();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT TOP 1 idTipoProducto FROM Producto WHERE idTipoProducto = @IdTipoProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdTipoProducto", dtoTipoPrenda.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tipoPrenda.Id = Convert.ToInt64(reader["idTipoProducto"]);
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return tipoPrenda != null && tipoPrenda.Id > 0;
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
                string sentenciaSql = @"UPDATE TipoProducto SET nombre = @nombre WHERE idTipoProducto = @idTipoProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@idTipoProducto", tipoPrenda.Id);
                cmd.Parameters.AddWithValue("@Nombre", tipoPrenda.Nombre);
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

        public bool NombreOcupado(DTOTipoPrenda dto)
        {
            List<TipoPrenda> prendas = new List<TipoPrenda>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM TipoProducto WHERE UPPER(nombre) = @Nombre AND idTipoProducto != @IdTipoProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Nombre", dto.Nombre.ToUpper());
                cmd.Parameters.AddWithValue("@IdTipoProducto", dto.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TipoPrenda prenda = new TipoPrenda();
                        prenda.Id = Convert.ToInt64(reader["idTipoProducto"]);
                        prenda.Nombre = reader["nombre"].ToString();
                        prenda.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                        prendas.Add(prenda);
                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);

                return prendas.Count() > 0;
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
            List<DTOTipoPrenda> tipos = new List<DTOTipoPrenda>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM TipoProducto where bajaLogica = 0";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TipoPrenda tipoPrenda = new TipoPrenda();

                        tipoPrenda.Id = Convert.ToInt64(reader["idTipoProducto"]);
                        tipoPrenda.Nombre = reader["nombre"].ToString();
                        tipoPrenda.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                        DTOTipoPrenda dtoTipoP = tipoPrenda.darDto();

                        tipos.Add(dtoTipoP);
                    }
                }
                trn.Rollback();
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

        public bool VerificarExistenciaCategoria(DTOTipoPrenda dtoTipoPrenda)
        {
            throw new NotImplementedException();
        }
    }
}
