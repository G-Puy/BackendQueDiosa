using DTOS;
using IRepositorios;
using Microsoft.AspNetCore.Mvc;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private IRepositorioVenta ManejadorVenta { get; set; }

        public VentaController([FromServices] IRepositorioVenta repInj)
        {
            this.ManejadorVenta = repInj;
        }

        [HttpPost("Alta")]
        public IActionResult Alta([FromBody] DTOVenta dto)
        {
            try
            {
                var resultado = ManejadorVenta.Alta(dto);
                if (resultado) return Ok(resultado);
                return BadRequest(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("TraerTodos")]
        public IActionResult TraerTodos()
        {
            try
            {
                List<DTOVenta> resultado = (List<DTOVenta>)this.ManejadorVenta.TraerTodos();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
