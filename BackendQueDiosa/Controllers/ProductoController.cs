using Conexiones;
using Dominio;
using DTOS;
using DTOS.DTOSProductoFrontBack;
using IRepositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private IRepositorioProducto ManejadorProducto { get; set; }
       private Logger log = new LoggerConfiguration().WriteTo.Console().CreateLogger();

    public ProductoController([FromServices] IRepositorioProducto repInj)
        {
            this.ManejadorProducto = repInj;
        }

        [HttpPost("alta")]
        public async Task<IActionResult> Alta([FromForm] IFormCollection dataEnvio)
        {
            try
            {
                
                var productoJson = dataEnvio["producto"].ToString();
                Console.WriteLine(productoJson);
                DTOProductoEnvio producto = JsonConvert.DeserializeObject<DTOProductoEnvio>(productoJson);
                var archivos = dataEnvio.Files;

                if (archivos.Count < 1) return BadRequest("No hay imagenes");

                DTOProducto dtoProducto = new DTOProducto();
                dtoProducto.Nombre = producto.Nombre;
                dtoProducto.PrecioAnterior = producto.PrecioAnterior;
                dtoProducto.Nuevo = producto.Nuevo;
                dtoProducto.Descripcion = producto.Descripcion;
                dtoProducto.PrecioActual = producto.PrecioActual;
                dtoProducto.IdTipoProducto = producto.IdTipoProducto;
                dtoProducto.BajaLogica = producto.BajaLogica;
                dtoProducto.GuiaTalles = producto.GuiaTalles;

                DTOStockEnvio stock = producto.Stock;

                foreach ( DTOTalleEnvio talle in stock.Talles)
                {
                    foreach (DTOColorEnvio color in talle.Colores)
                    {
                        DTOStock dtoStock =  new DTOStock();
                        dtoStock.IdProducto = stock.IdProducto;
                        dtoStock.IdColor = color.Id;
                        dtoStock.IdTalle = talle.Id;
                        dtoStock.Cantidad = color.Cantidad;

                        dtoProducto.Stocks.Add(dtoStock);
                    }
                }

                DTOProducto dtop = new DTOProducto();
                dtop.Nombre = producto.Nombre;
                dtop.Id = producto.Id;

                if (ManejadorProducto.NombreOcupado(dtop)) return BadRequest("Nombre en uso");


                Task<bool> resultadoAlta = this.ManejadorProducto.Alta(dtoProducto, archivos);

                if (resultadoAlta.Result) return Ok(true);
                else return BadRequest("Fallo al ingresar");
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [Authorize]
        [HttpPost("bajaLogica")]
        public IActionResult BajaLogica([FromBody] DTOProducto dtoProducto)
        {
            try
            {
                bool resultado = this.ManejadorProducto.BajaLogica(dtoProducto);

                if (resultado) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("buscarPorId")]
        public IActionResult BuscarPorId(int id)
        {
            try
            {
                DTOProducto dtoProducto = new DTOProducto();
                dtoProducto.Id = id;

                DTOProducto resultado = this.ManejadorProducto.BuscarPorId(dtoProducto).Result;

                if (resultado != null && resultado.Id > 0) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("buscarPorNombre")]
        public IActionResult BuscarPorNombre(string nombre)
        {
            try
            {
                DTOProducto dtoProducto = new DTOProducto();
                dtoProducto.Nombre = nombre;

                DTOProducto resultado = this.ManejadorProducto.BuscarPorNombre(dtoProducto).Result;

                if (resultado != null && resultado.Id > 0) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("eliminar")]
        public IActionResult Eliminar(long id)
        {
            try
            {
                DTOProducto dtoProducto = new DTOProducto();
                dtoProducto.Id = id;
                bool resultado = false;

                if (this.ManejadorProducto.EnUso(dtoProducto))
                {
                    resultado = this.ManejadorProducto.BajaLogica(dtoProducto);
                }
                else
                {
                    resultado = this.ManejadorProducto.Eliminar(dtoProducto).Result;
                }

                if (resultado) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut("modificar")]
        public async Task<IActionResult> Modificar([FromForm] IFormCollection dataEnvio)
        {
            try
            {
                var productoJson = dataEnvio["producto"].ToString();
                Console.WriteLine(productoJson);
                DTOProductoEnvio producto = JsonConvert.DeserializeObject<DTOProductoEnvio>(productoJson);
                var archivos = dataEnvio.Files;

                if (archivos.Count < 1) return BadRequest("No hay imagenes");

                DTOProducto dtoProducto = new DTOProducto();
                dtoProducto.Id = producto.Id;
                dtoProducto.Nombre = producto.Nombre;
                dtoProducto.PrecioAnterior = producto.PrecioAnterior;
                dtoProducto.Nuevo = producto.Nuevo;
                dtoProducto.Descripcion = producto.Descripcion;
                dtoProducto.PrecioActual = producto.PrecioActual;
                dtoProducto.IdTipoProducto = producto.IdTipoProducto;
                dtoProducto.BajaLogica = producto.BajaLogica;
                dtoProducto.GuiaTalles = producto.GuiaTalles;
                dtoProducto.VisibleEnWeb = producto.VisibleEnWeb;

                DTOStockEnvio stock = producto.Stock;

                foreach (DTOTalleEnvio talle in stock.Talles)
                {
                    foreach (DTOColorEnvio color in talle.Colores)
                    {
                        DTOStock dtoStock = new DTOStock();
                        dtoStock.Id = color.IdStock;
                        dtoStock.IdProducto = stock.IdProducto;
                        dtoStock.IdColor = color.Id;
                        dtoStock.IdTalle = talle.Id;
                        dtoStock.Cantidad = 0;

                        dtoProducto.Stocks.Add(dtoStock);
                    }
                }

                if (this.ManejadorProducto.NombreOcupado(dtoProducto))
                    return BadRequest("Ya existe nombre");

                bool resultado = this.ManejadorProducto.Modificar(dtoProducto, archivos).Result;

                if (resultado) return Ok(true);
                else return BadRequest("Fallo al modificar");

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("TraerTodos")]
        public IActionResult TraerTodos()
        {
            try
            {
                log.Information("LLego a Traer Todos");
                log.Debug("LLego a Traer Todos");
                List<DTOProductoEnviarAFRONT> resultado = (List<DTOProductoEnviarAFRONT>)this.ManejadorProducto.TraerTodos().Result;
                
                if (resultado != null) return Ok(resultado);
                else return BadRequest(resultado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
