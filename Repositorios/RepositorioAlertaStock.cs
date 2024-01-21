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
    public class RepositorioAlertaStock : RepositorioBase, IRepositorioAlertaStock
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Alta(DTOAlertaStock obj)
        {
            throw new NotImplementedException();
        }

        public bool BajaLogica(DTOAlertaStock obj)
        {
            throw new NotImplementedException();
        }

        public DTOAlertaStock BuscarPorId(DTOAlertaStock obj)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(DTOAlertaStock obj)
        {
            throw new NotImplementedException();
        }

        public bool Leer(long id)
        {
            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                string sentenciaSql = @"UPDATE AlertaStock SET leida = @Leida WHERE idAlertaStock = @IdAlertaStock";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Leida", true);
                cmd.Parameters.AddWithValue("@IdAlertaStock", id);
                cmd.Transaction = trn;
                cmd.ExecuteNonQuery();
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

        public bool Modificar(DTOAlertaStock obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DTOAlertaStock> TraerTodos()
        {
            List<DTOAlertaStock> dtos = new List<DTOAlertaStock>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM AlertaStock WHERE leida = @Leida";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                cmd.Parameters.AddWithValue("@Leida", false);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AlertaStock alerta = new AlertaStock();
                        alerta.Id = Convert.ToInt64(reader["idAlertaStock"]);
                        alerta.Leida = Convert.ToBoolean(reader["leida"]);
                        alerta.NombreProducto = Convert.ToString(reader["nombreProducto"]);
                        alerta.NombreTalle = Convert.ToString(reader["nombreTalle"]);
                        alerta.NombreColor = Convert.ToString(reader["nombreColor"]);
                        alerta.Cantidad = Convert.ToInt32(reader["cantidad"]);
                        dtos.Add(alerta.darDto());
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return dtos;
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
