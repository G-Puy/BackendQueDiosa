using Conexiones;
using DTOS;
using IRepositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private IRepositorioProducto ManejadorProducto { get; set; }
        private ServicioBlob ServicioBlob { get; set; }

        public ProductoController([FromServices] IRepositorioProducto repInj, ServicioBlob servicioBlob)
        {
            this.ManejadorProducto = repInj;
            ServicioBlob = servicioBlob;
        }

        [Authorize]
        [HttpPost("alta")]
        public async Task<IActionResult> Alta([FromBody] DTOProducto dtoProducto, List<IFormFile> imagenes)
        {
            try
            {
                if (this.ManejadorProducto.BuscarPorNombre(dtoProducto) != null) return BadRequest("Nombre ya existe");

                Task<bool> resultadoAlta = this.ManejadorProducto.Alta(dtoProducto, imagenes);

                if (resultadoAlta.Result) return Ok("Ingresado exitosamente");
                else return BadRequest("Fallo al ingresar");

            }
            catch (Exception ex)
            {
                throw ex;
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

                DTOProducto resultado = this.ManejadorProducto.BuscarPorId(dtoProducto);

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

                DTOProducto resultado = this.ManejadorProducto.BuscarPorNombre(dtoProducto);

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
                    resultado = this.ManejadorProducto.Eliminar(dtoProducto);
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

                bool resultado = this.ManejadorProducto.Modificar(dtoProducto, imagenes);

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
                List<DTOProducto> resultado = (List<DTOProducto>)this.ManejadorProducto.TraerTodos();
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
