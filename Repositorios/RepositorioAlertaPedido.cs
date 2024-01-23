using Conexiones;
using Dominio.Entidades;
using DTOS;
using DTOS.DTOSProductoFrontBack;
using Repositorios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IRepositorios
{
    public class RepositorioAlertaPedido : RepositorioBase, IRepositorioPedido
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Alta(DTOAlertaPedido obj)
        {
            throw new NotImplementedException();
        }

        public bool BajaLogica(DTOAlertaPedido obj)
        {
            throw new NotImplementedException();
        }

        public DTOAlertaPedido BuscarPorId(DTOAlertaPedido obj)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(DTOAlertaPedido obj)
        {
            throw new NotImplementedException();
        }

        public bool Realizado(long id)
        {
            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE AlertaPedido SET realizado = @Realizado WHERE idAlertaPedido = @IdAlertaPedido";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Realizado", true);
                cmd.Parameters.AddWithValue("@IdAlertaPedido", id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
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

        public bool Modificar(DTOAlertaPedido obj)
        {
            throw new NotImplementedException();
        }

        public List<DTOAlertaPedido> TraerFiltrado(DTOFiltroAlertasPedidos dtoFiltro)
        {
            List<DTOAlertaPedido> dtos = new List<DTOAlertaPedido>();
            List<AlertaPedido> alertas = new List<AlertaPedido>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                bool primera = true;
                string sentenciaSql = @"SELECT * FROM AlertaPedido a";
                if (dtoFiltro.envioRetiro != "" || dtoFiltro.nombre != "" || dtoFiltro.apellido != "")
                {
                    sentenciaSql += " JOIN Venta v on v.idVenta = a.idVenta";
                }
                if (dtoFiltro.IdVenta != -1)
                {
                    if (primera) { sentenciaSql += " WHERE"; primera = false; } else sentenciaSql += " AND";
                    sentenciaSql += " a.idVenta = @IdVenta";
                }
                if (dtoFiltro.realizado != "")
                {
                    if (primera) { sentenciaSql += " WHERE"; primera = false; } else sentenciaSql += " AND";
                    sentenciaSql += " a.realizado = @Realizado";
                }
                if (dtoFiltro.envioRetiro != "")
                {
                    if (primera) { sentenciaSql += " WHERE"; primera = false; } else sentenciaSql += " AND";
                    sentenciaSql += " v.envio = @Envio";
                }
                if (dtoFiltro.nombre != "")
                {
                    if (primera) { sentenciaSql += " WHERE"; primera = false; } else sentenciaSql += " AND";
                    sentenciaSql += " v.nombreComprador = @NombreComprador";
                }
                if (dtoFiltro.apellido != "")
                {
                    if (primera) { sentenciaSql += " WHERE"; primera = false; } else sentenciaSql += " AND";
                    sentenciaSql += " v.apellidoComprador = @ApellidoComprador";
                }

                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                if (dtoFiltro.IdVenta != -1) cmd.Parameters.AddWithValue("@IdVenta", dtoFiltro.IdVenta);
                if (dtoFiltro.realizado == "realizado" || dtoFiltro.realizado == "noRealizado")
                {
                    bool realizado = dtoFiltro.realizado.Equals("realizado");
                    cmd.Parameters.AddWithValue("@Realizado", realizado);
                }
                if (dtoFiltro.envioRetiro == "envio" || dtoFiltro.envioRetiro == "retiro")
                {
                    bool envio = dtoFiltro.envioRetiro.Equals("envio");
                    cmd.Parameters.AddWithValue("@Envio", envio);
                }
                if (dtoFiltro.nombre != "") cmd.Parameters.AddWithValue("@NombreComprador", dtoFiltro.nombre);
                if (dtoFiltro.apellido != "") cmd.Parameters.AddWithValue("@ApellidoComprador", dtoFiltro.apellido);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AlertaPedido alerta = new AlertaPedido();
                        alerta.Id = Convert.ToInt64(reader["idAlertaPedido"]);
                        alerta.IdVenta = Convert.ToInt64(reader["idVenta"]);
                        alerta.Realizado = Convert.ToBoolean(reader["realizado"]);
                        alertas.Add(alerta);
                    }
                }

                var secuenciaVenta = @"SELECT * FROM Venta WHERE idVenta = @IdVenta";
                foreach (AlertaPedido alerta in alertas)
                {
                    DTOAlertaPedido dto = alerta.darDto();

                    cmd.CommandText = secuenciaVenta;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdVenta", alerta.IdVenta);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dto.IdVenta = Convert.ToInt64(reader["idVenta"]);
                            dto.MontoTotal = Convert.ToDecimal(reader["montoTotal"]);
                            dto.Nombre = Convert.ToString(reader["nombreComprador"]);
                            dto.Direccion = Convert.ToString(reader["direccion"]);
                            dto.Telefono = Convert.ToString(reader["telefono"]);
                            dto.Apellido = Convert.ToString(reader["apellidoComprador"]);
                            dto.Envio = Convert.ToBoolean(reader["envio"]);
                        }
                    }

                    dtos.Add(dto);
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

        public IEnumerable<DTOAlertaPedido> TraerTodos()
        {
            List<DTOAlertaPedido> dtos = new List<DTOAlertaPedido>();
            List<AlertaPedido> alertas = new List<AlertaPedido>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM AlertaPedido";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AlertaPedido alerta = new AlertaPedido();
                        alerta.Id = Convert.ToInt64(reader["idAlertaPedido"]);
                        alerta.IdVenta = Convert.ToInt64(reader["idVenta"]);
                        alerta.Realizado = Convert.ToBoolean(reader["realizado"]);
                        alertas.Add(alerta);
                    }
                }

                var secuenciaVenta = @"SELECT * FROM Venta WHERE idVenta = @IdVenta";
                foreach (AlertaPedido alerta in alertas)
                {
                    DTOAlertaPedido dto = alerta.darDto();

                    cmd.CommandText = secuenciaVenta;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdVenta", alerta.IdVenta);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dto.IdVenta = Convert.ToInt64(reader["idVenta"]);
                            dto.MontoTotal = Convert.ToDecimal(reader["montoTotal"]);
                            dto.Nombre = Convert.ToString(reader["nombreComprador"]);
                            dto.Direccion = Convert.ToString(reader["direccion"]);
                            dto.Telefono = Convert.ToString(reader["telefono"]);
                            dto.Apellido = Convert.ToString(reader["apellidoComprador"]);
                            dto.Envio = Convert.ToBoolean(reader["envio"]);
                        }
                    }

                    dtos.Add(dto);
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
