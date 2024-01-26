using Conexiones;
using Dominio;
using Dominio.Entidades;
using DTOS;
using DTOS.DTOSProductoFrontBack;
using IRepositorios;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Core;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace Repositorios
{
    public class RepositorioProducto : RepositorioBase, IRepositorioProducto
    {
        private Logger log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
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
                string sentenciaProducto = @"INSERT INTO Producto VALUES (@Nombre, @Descripcion, @PrecioActual, @PrecioAnterior, @IdTipoProducto, @VisibleEnWeb, @Nuevo, @BajaLogica,@GuiaDeTalles);
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
                cmd.Parameters.AddWithValue("@GuiaDeTalles", producto.GuiaTalles);
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
                    string sentenciaImagen = @"INSERT INTO Imagen VALUES(@IdProducto, @Extension);
                                                SELECT CAST(Scope_IDentity() as int); ";

                    cmd.CommandText = sentenciaImagen;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdProducto", idGeneradoProducto);
                    cmd.Parameters.AddWithValue("@Extension", imagen.ContentType.Split("/").Last());
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
                string sentenciaSql = @"UPDATE Producto SET bajaLogica = @BajaLogica WHERE idProducto = @IdProducto";
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
                        producto.GuiaTalles = Convert.ToString(reader["guiaTalles"]);
                    }
                }

                string sentenciaImagenes = @"SELECT * FROM Imagenes WHERE idProducto = @IdProducto";
                cmd.CommandText = sentenciaImagenes;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IdProducto", obj.Id);

                List<Imagen> imagenes = new List<Imagen>();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Imagen imagen = new Imagen();
                        imagen.Id = Convert.ToInt64(reader["idImagen"]);
                        imagen.IdProducto = Convert.ToInt64(reader["idProducto"]);
                        imagen.Extension = Convert.ToString(reader["extension"]);
                        imagenes.Add(imagen);
                    }
                }

                foreach (Imagen imagen in imagenes)
                {
                    byte[] stream = await servicioBlob.GetBlobAsync($"{imagen.IdProducto}i{imagen.Id}");
                    DTOImagen dtoImagen = new DTOImagen();
                    dtoImagen.Imagen = stream;
                    dtoImagen.Extension = imagen.Extension;
                    producto.Imagenes.Add(dtoImagen);
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

        public List<DTOProducto> BuscarPorIds(List<DTOProducto> ids)
        {
            List<DTOProducto> productos = new List<DTOProducto>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Producto WHERE idProducto IN (";
                for (int i = 0; i < ids.Count; i++)
                {
                    if (i != 0)
                        sentenciaSql += ", ";

                    sentenciaSql += ids.ElementAt(i).Id;
                }
                sentenciaSql += ");";
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
                        productos.Add(producto.darDto());
                    }
                }

                trn.Commit();
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
                        producto.GuiaTalles = Convert.ToString(reader["guiaTalles"]);
                    }
                }

                string sentenciaImagenes = @"SELECT * FROM Imagenes WHERE idProducto = @IdProducto";
                cmd.CommandText = sentenciaImagenes;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IdProducto", dtoProducto.Id);

                List<Imagen> imagenes = new List<Imagen>();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Imagen imagen = new Imagen();
                        imagen.Id = Convert.ToInt64(reader["idImagen"]);
                        imagen.IdProducto = Convert.ToInt64(reader["idProducto"]);
                        imagen.Extension = Convert.ToString(reader["extension"]);
                        imagenes.Add(imagen);
                    }
                }

                foreach (Imagen imagen in imagenes)
                {
                    byte[] stream = await servicioBlob.GetBlobAsync($"{imagen.IdProducto}i{imagen.Id}");
                    DTOImagen dtoImagen = new DTOImagen();
                    dtoImagen.Imagen = stream;
                    dtoImagen.Extension = imagen.Extension;
                    producto.Imagenes.Add(dtoImagen);
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
                string sentenciaStock = @"DELETE FROM Stock WHERE idProducto = @IdProducto;";
                string sentenciaProducto = @"DELETE FROM Producto WHERE idProducto = @IdProducto;";

                SqlCommand cmd = new SqlCommand(sentenciaStock, cn);
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int affected = cmd.ExecuteNonQuery();

                string sentenciaImagenes = @"SELECT * FROM Imagen WHERE idProducto = @IdProducto";
                cmd.CommandText = sentenciaImagenes;
                cmd.Parameters.Clear();
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
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                affected = cmd.ExecuteNonQuery();

                foreach (Imagen imagen in imagenes)
                {
                    await servicioBlob.DeleteBlobAsync($"{imagen.IdProducto}i{imagen.Id}");
                }

                cmd.CommandText = sentenciaProducto;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                affected = cmd.ExecuteNonQuery();

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
                        producto.Id = Convert.ToInt64(reader["idVenta"]);
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

        public bool Existe(DTOProducto dTOProducto)
        {
            List<Producto> productos = new List<Producto>();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sentenciaSql = @"SELECT * FROM Producto WHERE nombre = @Nombre";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@Nombre", dTOProducto.Nombre);
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
                        productos.Add(producto);
                    }
                }
                trn.Commit();
                manejadorConexion.CerrarConexionConClose(cn);
                if (productos.Count > 1) return false;
                if (productos.Count == 0) return true;
                Producto productoAValidar = productos[0];
                return productoAValidar.Id == dTOProducto.Id;
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
                string sentenciaSql = @"UPDATE Producto SET nombre = @Nombre, descripcion = @Descripcion, precioActual = @PrecioActual, precioAnterior = @PrecioAnterior, idTipoProducto = @IdTipoProducto, visibleEnWeb = @VisibleEnWeb, nuevo = @Nuevo, guiaTalles = @GuiaTalles WHERE idProducto = @IdProducto";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                cmd.Parameters.AddWithValue("@PrecioActual", producto.PrecioActual);
                cmd.Parameters.AddWithValue("@PrecioAnterior", producto.PrecioAnterior);
                cmd.Parameters.AddWithValue("@IdTipoProducto", producto.IdTipoProducto);
                cmd.Parameters.AddWithValue("@VisibleEnWeb", producto.VisibleEnWeb);
                cmd.Parameters.AddWithValue("@Nuevo", producto.Nuevo);
                cmd.Parameters.AddWithValue("@GuiaTalles", producto.GuiaTalles);
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                int idGenerado = cmd.ExecuteNonQuery();

                if (imagenesDTO.Count == 0 || !imagenesDTO[0].FileName.Contains("NOMODIFICAR"))
                {

                    string sentenciaImagenes = @"SELECT * FROM Imagen WHERE idProducto = @IdProducto";
                    cmd.CommandText = sentenciaImagenes;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdProducto", producto.Id);

                    List<Imagen> imagenes = new List<Imagen>();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Imagen imagen = new Imagen();
                            imagen.Id = Convert.ToInt64(reader["idImagen"]);
                            imagen.IdProducto = Convert.ToInt64(reader["idProducto"]);
                            imagen.Extension = Convert.ToString(reader["extension"]);
                            imagenes.Add(imagen);
                        }
                    }

                    string sentenciaEliminarImagenes = @"DELETE FROM Imagen WHERE idProducto = @IdProducto";
                    cmd.CommandText = sentenciaEliminarImagenes;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                    int affected = cmd.ExecuteNonQuery();

                    foreach (Imagen imagen in imagenes)
                    {
                        await servicioBlob.DeleteBlobAsync($"{imagen.IdProducto}i{imagen.Id}");
                    }

                    foreach (var imagen in imagenesDTO)
                    {
                        string sentenciaImagen = @"INSERT INTO Imagen VALUES(@IdProducto, @Extension);
                                                SELECT CAST(Scope_IDentity() as int)";
                        cmd.CommandText = sentenciaImagen;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                        cmd.Parameters.AddWithValue("@Extension", imagen.ContentType.Split("/").Last());
                        idGenerado = (int)cmd.ExecuteScalar();

                        using var stream = imagen.OpenReadStream();
                        await servicioBlob.UploadBlobAsync($"{producto.Id}i{idGenerado}", stream);
                    }

                }
                string sentenciaTrearStock = @"SELECT * FROM Stock WHERE idProducto = @IdProducto";
                cmd.CommandText = sentenciaTrearStock;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IdProducto", producto.Id);

                List<Stock> stocks = new List<Stock>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Stock stock = new Stock();
                        stock.Id = Convert.ToInt64(reader["idStock"]);
                        stock.IdProducto = Convert.ToInt64(reader["idProducto"]);
                        stock.IdColor = Convert.ToInt64(reader["idColor"]);
                        stock.IdTalle = Convert.ToInt64(reader["idTalle"]);
                        stock.Cantidad = Convert.ToInt32(reader["cantidad"]);
                        stocks.Add(stock);
                    }
                }

                int affected2 = 0;
                foreach (Stock stock in stocks)
                {
                    bool esta = false;
                    foreach (Stock st in producto.Stocks)
                    {
                        if (stock.IdTalle == st.IdTalle && stock.IdColor == st.IdColor && stock.IdProducto == producto.Id) { esta = true; break; }
                    }

                    if (!esta)
                    {
                        string sentenciaStock = @"DELETE FROM Stock WHERE idStock = @IdStock";
                        cmd.CommandText = sentenciaStock;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@IdStock", stock.Id);
                        affected2 = cmd.ExecuteNonQuery();
                    }
                }

                foreach (Stock stock in producto.Stocks)
                {
                    bool esta = false;
                    foreach (Stock st in stocks)
                    {
                        log.Debug($"Talle: ${stock.IdTalle} == ${st.IdTalle}");
                        log.Debug($"Color: ${stock.IdColor} == ${st.IdColor}");
                        log.Debug($"Pro: ${producto.Id} == ${st.IdProducto}");
                        if (stock.IdTalle == st.IdTalle && stock.IdColor == st.IdColor && producto.Id == st.IdProducto) { esta = true; break; }
                    }

                    if (!esta)
                    {
                        string sentenciaAgregarStock = @"INSERT INTO Stock VALUES(@IdProducto, @IdColor, @IdTalle, @Cantidad)
                                            SELECT CAST(Scope_IDentity() as int)";
                        cmd.CommandText = sentenciaAgregarStock;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                        cmd.Parameters.AddWithValue("@IdColor", stock.IdColor);
                        cmd.Parameters.AddWithValue("@IdTalle", stock.IdTalle);
                        cmd.Parameters.AddWithValue("@Cantidad", 0);
                        affected2 = (int)cmd.ExecuteScalar();
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

        public async Task<IEnumerable<DTOProductoEnviarAFRONT>> TraerTodos()
        {
            List<DTOProductoEnviarAFRONT> productos = new List<DTOProductoEnviarAFRONT>();

            cn = manejadorConexion.CrearConexion();
            log.Information(cn.ToString());
            SqlTransaction trn = null;

            try
            {
                string sentenciaSql = @"SELECT * FROM Producto WHERE bajaLogica = 0;";
                SqlCommand cmd = new SqlCommand(sentenciaSql, cn);
                log.Information(cmd.ToString());
                manejadorConexion.AbrirConexion(cn);
                log.Information(cn.ToString());
                trn = cn.BeginTransaction();
                cmd.Transaction = trn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DTOProductoEnviarAFRONT producto = new DTOProductoEnviarAFRONT
                        {
                            Id = Convert.ToInt64(reader["idProducto"]),
                            Nombre = Convert.ToString(reader["nombre"]),
                            Descripcion = Convert.ToString(reader["descripcion"]),
                            PrecioActual = Convert.ToDouble(reader["precioActual"]),
                            PrecioAnterior = Convert.ToDouble(reader["precioAnterior"]),
                            IdTipoProducto = Convert.ToInt64(reader["idTipoProducto"]),
                            VisibleEnWeb = Convert.ToBoolean(reader["visibleEnWeb"]),
                            Nuevo = Convert.ToBoolean(reader["nuevo"]),
                            BajaLogica = Convert.ToBoolean(reader["bajaLogica"]),
                            GuiaTalles = Convert.ToString(reader["guiaTalles"])
                        };

                        //DTOProducto dtoTipoT = producto.darDto();

                        productos.Add(producto);
                    }
                }



                foreach (DTOProductoEnviarAFRONT dtoProdEnvioFront in productos)
                {
                    cmd.CommandText = @"SELECT nombre FROM TipoProducto where idTipoProducto = @IdTipoProducto";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdTIpoProducto", dtoProdEnvioFront.IdTipoProducto);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dtoProdEnvioFront.TipoProductoNombre = Convert.ToString(reader["nombre"]);
                        }
                    }

                    string sentenciaImagenes = @"SELECT * FROM Imagen WHERE idProducto = @IdProducto;";
                    cmd.CommandText = sentenciaImagenes;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdProducto", dtoProdEnvioFront.Id);

                    List<Imagen> imagenes = new List<Imagen>();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Imagen imagen = new Imagen();
                            imagen.Id = Convert.ToInt64(reader["idImagen"]);
                            imagen.IdProducto = Convert.ToInt64(reader["idProducto"]);
                            imagen.Extension = Convert.ToString(reader["extension"]);
                            imagenes.Add(imagen);
                        }
                    }

                    foreach (Imagen imagen in imagenes)
                    {
                        byte[] stream = await servicioBlob.GetBlobAsync($"{imagen.IdProducto}i{imagen.Id}");
                        DTOImagen dtoImagen = new DTOImagen();
                        dtoImagen.Imagen = stream;
                        dtoImagen.Extension = imagen.Extension;
                        dtoProdEnvioFront.Imagenes.Add(dtoImagen);
                    }

                    string sentenciaStock = @"SELECT Stock.idStock, Stock.idProducto, Stock.idColor, Stock.idTalle, Stock.cantidad, 
                                             Talle.nombre as nombreTalle, Color.nombre as nombreColor FROM Stock 
                                             inner  join Talle on Stock.idTalle =Talle.idTalle 
                                             inner join Color on Stock.idColor = Color.idColor
                                             where idProducto = @IdProducto;";
                    cmd.CommandText = sentenciaStock;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdProducto", dtoProdEnvioFront.Id);
                    List<DTOStockTalleColorEnvioAFront> listaStocks = new List<DTOStockTalleColorEnvioAFront>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DTOStockTalleColorEnvioAFront stock = new DTOStockTalleColorEnvioAFront();
                            stock.Id = Convert.ToInt64(reader["idStock"]);
                            stock.IdProducto = Convert.ToInt64(reader["idProducto"]);
                            stock.IdTalle = Convert.ToInt64(reader["idTalle"]);
                            stock.IdColor = Convert.ToInt64(reader["idColor"]);
                            stock.Cantidad = Convert.ToInt32(reader["cantidad"]);
                            stock.NombreTalle = Convert.ToString(reader["nombreTalle"]);
                            stock.NombreColor = Convert.ToString(reader["nombreColor"]);

                            listaStocks.Add(stock);
                        }
                    }

                    var cantidadTotal = 0;

                    dtoProdEnvioFront.Stock.IdProducto = dtoProdEnvioFront.Id;
                    long idTalleActual = 0;
                    foreach (DTOStockTalleColorEnvioAFront stockActual in listaStocks)
                    {
                        if (idTalleActual != stockActual.IdTalle)
                        {
                            //CARGAR TALLE
                            DTOTalleEnvio dtoTalleEnv = new DTOTalleEnvio();
                            dtoTalleEnv.NombreTalle = stockActual.NombreTalle;
                            dtoTalleEnv.Id = stockActual.IdTalle;
                            dtoProdEnvioFront.Stock.Talles.Add(dtoTalleEnv);
                        }
                        idTalleActual = stockActual.IdTalle;

                        //CARGAR COLOR
                        DTOColorEnvio dTOColorEnvio = new DTOColorEnvio();
                        dTOColorEnvio.NombreColor = stockActual.NombreColor;
                        cantidadTotal += stockActual.Cantidad;
                        dTOColorEnvio.Cantidad = stockActual.Cantidad;
                        dTOColorEnvio.Id = stockActual.IdColor;
                        dTOColorEnvio.IdStock = stockActual.Id;

                        foreach (DTOTalleEnvio talleParaAgregarElColor in dtoProdEnvioFront.Stock.Talles)
                        {
                            if (talleParaAgregarElColor.Id == stockActual.IdTalle)
                            {
                                talleParaAgregarElColor.Colores.Add(dTOColorEnvio);
                                talleParaAgregarElColor.Cantidad += stockActual.Cantidad;
                            }
                        }
                    }

                    dtoProdEnvioFront.Stock.Cantidad = cantidadTotal;
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

    }
}
