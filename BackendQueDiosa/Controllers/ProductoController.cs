using Conexiones;
using DTOS;
using DTOS.DTOSProductoFrontBack;
using IRepositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private IRepositorioProducto ManejadorProducto { get; set; }

        public ProductoController([FromServices] IRepositorioProducto repInj)
        {
            this.ManejadorProducto = repInj;
        }

        //[Authorize]
        //[HttpPost("alta")]
        //public async Task<IActionResult> Alta([FromBody] DTOProducto dtoProducto, List<IFormFile> imagenes)
        //{
        //    try
        //    {
        //        if (this.ManejadorProducto.BuscarPorNombre(dtoProducto) != null) return BadRequest("Nombre ya existe");

        //        Task<bool> resultadoAlta = this.ManejadorProducto.Alta(dtoProducto, imagenes);

        //        if (resultadoAlta.Result) return Ok("Ingresado exitosamente");
        //        else return BadRequest("Fallo al ingresar");

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        [HttpPost("alta")]
        public async Task<IActionResult> CargarProducto([FromForm] IFormCollection dataEnvio)
        {
            try
            {
                var productoJson = dataEnvio["producto"].ToString();
                Console.WriteLine(productoJson);
                DTOProductoEnvio producto = JsonConvert.DeserializeObject<DTOProductoEnvio>(productoJson);
                var archivos = dataEnvio.Files;

                // Aquí, maneja la lógica para guardar las imágenes y el producto

                return Ok();
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

        [Authorize]
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
        public IActionResult Modificar([FromBody] DTOProducto dtoProducto, List<IFormFile> imagenes)
        {
            try
            {
                if (this.ManejadorProducto.BuscarPorNombre(dtoProducto) != null)
                    return BadRequest("Ya existe nombre");

                bool resultado = this.ManejadorProducto.Modificar(dtoProducto, imagenes).Result;

                if (resultado) return Ok("Modificado exitosamente");
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
                List<DTOProducto> resultado = (List<DTOProducto>)this.ManejadorProducto.TraerTodos().Result;
                if (resultado.Count > 0) return Ok(resultado);
                else return BadRequest(false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //EndPoints de prueba

        [HttpGet("PRUEBATraerImagenes")]
        public IActionResult TraerImagenes(int idProducto)
        {
            try
            {
                DTOProducto resultado = this.ManejadorProducto.TraerTodosImagenes(idProducto).Result;
                if (resultado.Imagenes.Count > 0) return Ok(resultado);
                else return BadRequest(false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("PRUEBAInsertarEnBlob")]
        public IActionResult InsertarEnBlob(List<IFormFile> imagenes, int idProducto)
        {
            try
            {
                bool resultado = this.ManejadorProducto.InsertarEnBlob(imagenes, idProducto).Result;
                if (resultado) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("PRUEBAInsertarEnBlobSINBD")]
        public IActionResult InsertarEnBlobSINBD(List<IFormFile> imagenes, int idProducto)
        {
            try
            {
                //TODO: ARREGLAR
                //bool resultado = this.ManejadorProducto.InsertarEnBlobSINBD(imagenes, idProducto).Result;
                bool resultado = false;
                if (resultado) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("PRUEBATraerStreams")]
        public IActionResult TraerStreams(List<IFormFile> imagenes)
        {
            try
            {
                List<Stream> resultado = new List<Stream>();

                foreach (var imagen in imagenes)
                {
                    using var stream = imagen.OpenReadStream();
                    resultado.Add(stream);
                }

                if (resultado.Count > 0) return Ok(resultado);
                else return BadRequest(false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
