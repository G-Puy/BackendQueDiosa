using DTOS;
using IRepositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private IRepositorioStock ManejadorStock { get; set; }

        public StockController([FromServices] IRepositorioStock repInj)
        {
            this.ManejadorStock = repInj;
        }

        [Authorize]
        [HttpPost("alta")]
        public IActionResult Alta([FromBody] DTOStock dtoStock)
        {
            try
            {
                bool resultadoAlta = this.ManejadorStock.Alta(dtoStock);

                if (resultadoAlta) return Ok(resultadoAlta);
                else return BadRequest(resultadoAlta);

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
                DTOStock dtoStock = new DTOStock();
                dtoStock.Id = id;

                DTOStock resultado = this.ManejadorStock.BuscarPorId(dtoStock);

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
                DTOStock dtoStock = new DTOStock();
                dtoStock.Id = id;

                bool resultado = this.ManejadorStock.Eliminar(dtoStock);

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
        public IActionResult Modificar([FromBody] DTOStock dtoStock)
        {
            try
            {
                bool resultado = this.ManejadorStock.Modificar(dtoStock);

                if (resultado) return Ok(resultado);
                else return BadRequest(resultado);

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
                List<DTOStock> resultado = (List<DTOStock>)this.ManejadorStock.TraerTodos();
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
