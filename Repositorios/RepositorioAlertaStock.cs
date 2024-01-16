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

        public bool Modificar(DTOAlertaStock obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DTOAlertaStock> TraerTodos()
        {
            List<DTOAlertaStock> alertas = new List<DTOAlertaStock>();

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
                        DTOAlertaStock dtoTipoT = alerta.darDto();
                        alertas.Add(dtoTipoT);
                    }
                }
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                return (IEnumerable<DTOAlertaStock>)alertas;
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
