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
                string sentenciaSql = @"UPDATE AlertaStock SET leida = @Leida WHERE idAlertaStock = @IdAlertaStock";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Leida",true);
                cmd.Parameters.AddWithValue("@IdAlertaStock", id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                cmd.ExecuteNonQuery();
                trn.Rollback();
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
            List<AlertaStock> alertas = new List<AlertaStock>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM AlertaStock";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AlertaStock alerta = new AlertaStock();
                        alerta.Id = Convert.ToInt64(reader["idAlertaStock"]);
                        alerta.IdStock = Convert.ToInt64(reader["idStock"]);
                        alerta.Descripcion = Convert.ToString(reader["descripcion"]);
                        alerta.Leida = Convert.ToBoolean(reader["leida"]);
                        alertas.Add(alerta);
                    }
                }

                cmd.CommandText = @"SELECT * FROM Stock WHERE idStock = @IdStock";
                foreach (AlertaStock alerta in alertas)
                {
                    DTOAlertaStock dto = alerta.darDto();

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdStock", alerta.IdStock);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Stock stock = new Stock();
                            stock.Id = Convert.ToInt64(reader["idStock"]);
                            stock.IdProducto = Convert.ToInt64(reader["idProducto"]);
                            stock.IdTalle = Convert.ToInt64(reader["idTalle"]);
                            stock.IdColor = Convert.ToInt64(reader["idColor"]);
                            stock.Cantidad = Convert.ToInt32(reader["cantidad"]);
                            dto.stock = stock.darDto();
                        }
                    }

                    dtos.Add(dto);
                }
                trn.Rollback();
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
