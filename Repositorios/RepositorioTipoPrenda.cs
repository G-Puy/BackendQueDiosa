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
            TipoPrenda categoria = new TipoPrenda();
            categoria.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"INSERT INTO Categoria VALUES(@NombreTipoPrenda,@BajaLogica)
                                    SELECT CAST(Scope_IDentity() as int)";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@NombreTipoPrenda", categoria.NombreTipoPrenda);
                cmd.Parameters.AddWithValue("@BajaLogica", categoria.BajaLogica);
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
            throw new NotImplementedException();
        }

        public DTOTipoPrenda BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(DTOTipoPrenda obj)
        {
            throw new NotImplementedException();
        }

        public bool Modificar(DTOTipoPrenda obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DTOTipoPrenda> TraerTodos()
        {
            throw new NotImplementedException();
        }

        public bool VerificarExistenciaCategoria(DTOTipoPrenda DTOCategoria)
        {
            throw new NotImplementedException();
        }
    }
}
