using Conexiones;
using Dominio;
using Dominio.Entidades;
using DTOS;
using IRepositorios;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

namespace Repositorios
{
    public class RepositorioProducto : RepositorioBase, IRepositorioProducto
    {
        private ServicioBlobAzure servicioBlob = new ServicioBlobAzure();

        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public async Task<bool> Alta(DTOProducto obj, IFormFileCollection imagenes)
        {
            Producto producto = new Producto();
            producto.cargarDeDTO(obj);
            

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaProducto = @"INSERT INTO Producto VALUES (@Nombre, @Descripcion, @PrecioActual, @PrecioAnterior, @IdTipoProducto, @VisibleEnWeb, @Nuevo, @BajaLogica);
                                            SELECT CAST(Scope_IDentity() as int);";
                
                SqlCommand cmd = new SqlCommand(sentenciaProducto, cn);
                cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                cmd.Parameters.AddWithValue("@PrecioActual", producto.PrecioActual);
                cmd.Parameters.AddWithValue("@PrecioAnterior", producto.PrecioAnterior);
                cmd.Parameters.AddWithValue("@IdTipoProducto", producto.IdTipoProducto);
                cmd.Parameters.AddWithValue("@VisibleEnWeb", true);
                cmd.Parameters.AddWithValue("@Nuevo", false);
                cmd.Parameters.AddWithValue("@BajaLogica", false);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int idGeneradoProducto = (int)cmd.ExecuteScalar();

                if (producto.Stocks.Count > 0)
                {
                    foreach (var stock in producto.Stocks)
                    {
                        string sentenciaStock = @"INSERT INTO Stock VALUES(@IdProducto, @IdColor, @IdTalle, @Cantidad);
                                            SELECT CAST(Scope_IDentity() as int);";
                        cmd.CommandText = sentenciaStock;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@IdProducto", idGeneradoProducto);
                        cmd.Parameters.AddWithValue("@IdColor", stock.IdColor);
                        cmd.Parameters.AddWithValue("@IdTalle", stock.IdTalle);
                        cmd.Parameters.AddWithValue("@Cantidad", stock.Cantidad);
                        int idGeneradoStock = (int)cmd.ExecuteScalar();
                    }
                }

                foreach (var imagen in imagenes)
                {
                    string sentenciaImagen = @"INSERT INTO Imagen VALUES(@IdProducto);
                                                SELECT CAST(Scope_IDentity() as int); ";

                    cmd.CommandText = sentenciaImagen;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdProducto", idGeneradoProducto);
                    int idGeneradoImagen = (int)cmd.ExecuteScalar();

                    using var stream = imagen.OpenReadStream();
                    await servicioBlob.UploadBlobAsync($"{idGeneradoProducto}i{idGeneradoImagen}", stream);
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

        public bool BajaLogica(DTOProducto obj)
        {
            Producto producto = new Producto();
            producto.cargarDeDTO(obj);


            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE TABLE Producto SET bajaLogica = @BajaLogica WHERE idProducto = @IdProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
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

        public async Task<DTOProducto> BuscarPorId(DTOProducto obj)
        {
            Producto producto = new Producto();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Producto WHERE idProducto = @IdProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdProducto", obj.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        producto.Id = Convert.ToInt64(reader["idProducto"]);
                        producto.Nombre = Convert.ToString(reader["nombre"]);
                        producto.Descripcion = Convert.ToString(reader["descripcion"]);
                        producto.PrecioActual = Convert.ToDouble(reader["precioActual"]);
                        producto.PrecioAnterior = Convert.ToDouble(reader["precioAnterior"]);
                        producto.IdTipoProducto = Convert.ToInt64(reader["idTipoProducto"]);
                        producto.VisibleEnWeb = Convert.ToBoolean(reader["visibleEnWeb"]);
                        producto.Nuevo = Convert.ToBoolean(reader["nuevo"]);
                        producto.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                    }
                }

                string sentenciaImagenes = @"SELECT * FROM Imagenes WHERE idProducto = @IdProducto";
                cmd.CommandText = sentenciaImagenes;
                cmd.Parameters.AddWithValue("@IdProducto", obj.Id);

                List<Imagen> imagenes = new List<Imagen>();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Imagen imagen = new Imagen();
                        imagen.Id = Convert.ToInt64(reader["idImagen"]);
                        imagen.IdProducto = Convert.ToInt64(reader["idProducto"]);
                        imagenes.Add(imagen);
                    }
                }

                foreach (Imagen imagen in imagenes)
                {
                    using Stream stream = await servicioBlob.GetBlobAsync($"{imagen.IdProducto}i{imagen.Id}");
                    producto.Imagenes.Add(stream);
                }

                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);

                if (producto == null || producto.Id == 0)
                {
                    return null;
                }

                return producto.darDto();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public async Task<DTOProducto> BuscarPorNombre(DTOProducto dtoProducto)
        {
            Producto producto = new Producto();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Producto WHERE UPPER(nombre) = @Nombre";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Nombre", dtoProducto.Nombre.ToUpper());
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        producto.Id = Convert.ToInt64(reader["idProducto"]);
                        producto.Nombre = Convert.ToString(reader["nombre"]);
                        producto.Descripcion = Convert.ToString(reader["descripcion"]);
                        producto.PrecioActual = Convert.ToDouble(reader["precioActual"]);
                        producto.PrecioAnterior = Convert.ToDouble(reader["precioAnterior"]);
                        producto.IdTipoProducto = Convert.ToInt64(reader["idTipoProducto"]);
                        producto.VisibleEnWeb = Convert.ToBoolean(reader["visibleEnWeb"]);
                        producto.Nuevo = Convert.ToBoolean(reader["nuevo"]);
                        producto.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                    }
                }

                string sentenciaImagenes = @"SELECT * FROM Imagenes WHERE idProducto = @IdProducto";
                cmd.CommandText = sentenciaImagenes;
                cmd.Parameters.AddWithValue("@IdProducto", dtoProducto.Id);

                List<Imagen> imagenes = new List<Imagen>();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Imagen imagen = new Imagen();
                        imagen.Id = Convert.ToInt64(reader["idImagen"]);
                        imagen.IdProducto = Convert.ToInt64(reader["idProducto"]);
                        imagenes.Add(imagen);
                    }
                }

                foreach (Imagen imagen in imagenes)
                {
                    using Stream stream = await servicioBlob.GetBlobAsync($"{imagen.IdProducto}i{imagen.Id}");
                    producto.Imagenes.Add(stream);
                }

                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);

                if (producto == null || producto.Id == 0)
                {
                    return null;
                }

                return producto.darDto();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public async Task<bool> Eliminar(DTOProducto obj)
        {
            Producto producto = new Producto();
            producto.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaAlerta = @"DELETE FROM AlertaStock WHERE idStock IN (SELECT idStock FROM Stock WHERE idProducto = @IdProducto)";
                string sentenciaStock = @"DELETE From Stock WHERE idProducto = @IdProducto";
                string sentenciaProducto = @"DELETE FROM Producto WHERE idProducto = @IdProducto";

                SqlCommand cmd = new SqlCommand(sentenciaAlerta, cn);
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int affected = cmd.ExecuteNonQuery();

                cmd.CommandText = sentenciaStock;
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                affected = cmd.ExecuteNonQuery();

                cmd.CommandText = sentenciaProducto;
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                affected = cmd.ExecuteNonQuery();

                string sentenciaImagenes = @"SELECT * FROM Imagenes WHERE idProducto = @IdProducto";
                cmd.CommandText = sentenciaImagenes;
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);

                List<Imagen> imagenes = new List<Imagen>();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Imagen imagen = new Imagen();
                        imagen.Id = Convert.ToInt64(reader["idImagen"]);
                        imagen.IdProducto = Convert.ToInt64(reader["idProducto"]);
                        imagenes.Add(imagen);
                    }
                }

                string sentenciaEliminarImagenes = @"DELETE FROM Imagen WHERE idProducto = @IdProducto";
                cmd.CommandText = sentenciaEliminarImagenes;
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                affected = cmd.ExecuteNonQuery();

                foreach (Imagen imagen in imagenes)
                {
                    await servicioBlob.DeleteBlobAsync($"{imagen.IdProducto}i{imagen.Id}");
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

        public bool EnUso(DTOProducto dtoProducto)
        {
            Producto producto = new Producto();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT TOP 1 idVenta FROM VentaProducto WHERE idProducto = @IdProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdProducto", dtoProducto.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        producto.Id = Convert.ToInt64(reader["idProducto"]);
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                return producto != null && producto.Id > 0;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public async Task<bool> Modificar(DTOProducto obj, IFormFileCollection imagenesDTO)
        {
            Producto producto = new Producto();
            producto.cargarDeDTO(obj);

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"UPDATE Producto SET nombre = @Nombre, descripcion = @Descripcion, precioActual = @PrecioActual, precioAnterior = @PrecioAnterior, idTipoProducto = @IdTipoProducto, visibleEnWeb = @VisibleEnWeb, nuevo = @Nuevo WHERE idProducto = @IdProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                cmd.Parameters.AddWithValue("@PrecioActual", producto.PrecioActual);
                cmd.Parameters.AddWithValue("@PrecioAnterior", producto.PrecioAnterior);
                cmd.Parameters.AddWithValue("@IdTipoProducto", producto.IdTipoProducto);
                cmd.Parameters.AddWithValue("@VisibleEnWeb", producto.VisibleEnWeb);
                cmd.Parameters.AddWithValue("@Nuevo", producto.Nuevo);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int idGenerado = cmd.ExecuteNonQuery();

                string sentenciaImagenes = @"SELECT * FROM Imagenes WHERE idProducto = @IdProducto";
                cmd.CommandText = sentenciaImagenes;
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);

                List<Imagen> imagenes = new List<Imagen>();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Imagen imagen = new Imagen();
                        imagen.Id = Convert.ToInt64(reader["idImagen"]);
                        imagen.IdProducto = Convert.ToInt64(reader["idProducto"]);
                        imagenes.Add(imagen);
                    }
                }

                string sentenciaEliminarImagenes = @"DELETE FROM Imagen WHERE idProducto = @IdProducto";
                cmd.CommandText = sentenciaEliminarImagenes;
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                int affected = cmd.ExecuteNonQuery();

                foreach (Imagen imagen in imagenes)
                {
                    await servicioBlob.DeleteBlobAsync($"{imagen.IdProducto}i{imagen.Id}");
                }

                foreach (var imagen in imagenesDTO)
                {
                    string sentenciaImagen = @"INSERT INTO Imagen VALUES(@IdProducto)
                                                SELECT CAST(Scope_IDentity as int)";
                    cmd.CommandText = sentenciaImagen;
                    cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                    idGenerado = (int)cmd.ExecuteScalar();

                    using var stream = imagen.OpenReadStream();
                    await servicioBlob.UploadBlobAsync($"{producto.Id}i{idGenerado}", stream);
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

        public async Task<IEnumerable<DTOProducto>> TraerTodos()
        {
            List<DTOProducto> productos = new List<DTOProducto>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Producto WHERE bajaLogica = 0;";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Producto producto = new Producto();

                        producto.Id = Convert.ToInt64(reader["idProducto"]);
                        producto.Nombre = Convert.ToString(reader["nombre"]);
                        producto.Descripcion = Convert.ToString(reader["descripcion"]);
                        producto.PrecioActual = Convert.ToDouble(reader["precioActual"]);
                        producto.PrecioAnterior = Convert.ToDouble(reader["precioAnterior"]);
                        producto.IdTipoProducto = Convert.ToInt64(reader["idTipoProducto"]);
                        producto.VisibleEnWeb = Convert.ToBoolean(reader["visibleEnWeb"]);
                        producto.Nuevo = Convert.ToBoolean(reader["nuevo"]);
                        producto.BajaLogica = Convert.ToBoolean(reader["bajaLogica"]);
                        producto.GuiaTalles = Convert.ToString(reader["guiaTalles"]);

                        DTOProducto dtoTipoT = producto.darDto();

                        productos.Add(dtoTipoT);
                    }
                }

                foreach (DTOProducto producto in productos)
                {
                    string sentenciaImagenes = @"SELECT * FROM Imagenes WHERE idProducto = @IdProducto";
                    cmd.CommandText = sentenciaImagenes;
                    cmd.Parameters.AddWithValue("@IdProducto", producto.Id);

                    List<Imagen> imagenes = new List<Imagen>();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Imagen imagen = new Imagen();
                            imagen.Id = Convert.ToInt64(reader["idImagen"]);
                            imagen.IdProducto = Convert.ToInt64(reader["idProducto"]);
                            imagenes.Add(imagen);
                        }
                    }

                    foreach (Imagen imagen in imagenes)
                    {
                        using Stream stream = await servicioBlob.GetBlobAsync($"{imagen.IdProducto}i{imagen.Id}");
                        producto.Imagenes.Add(stream);
                    }
                }

                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                return productos;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public async Task<DTOProducto> TraerTodosImagenes(int idProducto)
        {
            DTOProducto producto = new DTOProducto();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                 string sentenciaImagenes = @"SELECT * FROM Imagen WHERE idProducto = @IdProducto";
                SqlCommand cmd = new SqlCommand(sentenciaImagenes, cn);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);

                    List<Imagen> imagenes = new List<Imagen>();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Imagen imagen = new Imagen();
                            imagen.Id = Convert.ToInt64(reader["idImagen"]);
                            imagen.IdProducto = Convert.ToInt64(reader["idProducto"]);
                            imagenes.Add(imagen);
                        }
                    }

                    foreach (Imagen imagen in imagenes)
                    {
                        using Stream stream = await servicioBlob.GetBlobAsync($"{imagen.IdProducto}i{imagen.Id}");
                        producto.Imagenes.Add(stream);
                    }
                

                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                return producto;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                manejadorConexion.CerrarConexionConClose(cn);
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }

        public async Task<bool> InsertarEnBlob(List<IFormFile> imagenes, int productoId)
        {
            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;

                int idGenerado = 0;
                foreach (var imagen in imagenes)
                {
                    string sentenciaImagen = @"INSERT INTO Imagen VALUES(@IdProducto);
                                               SELECT CAST(Scope_IDentity() as int);";
                    cmd.CommandText = sentenciaImagen;
                    cmd.Parameters.AddWithValue("@IdProducto", productoId);
                    idGenerado = (int)cmd.ExecuteScalar();

                    using var stream = imagen.OpenReadStream();
                    await servicioBlob.UploadBlobAsync($"{productoId}i{idGenerado}", stream);
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

        public async Task<bool> InsertarEnBlobSINBD(List<IFormFile> imagenes, int productoId)
        {
           try {
                foreach (IFormFile imagen in imagenes)
                {
                    using var stream = imagen.OpenReadStream();
                    await servicioBlob.UploadBlobAsync($"{productoId}i{1}", stream);
                }
                return true;
            }
            catch (Exception ex)
            {
                this.DescripcionError = ex.Message;
                throw ex;
            }
        }
    }
}
