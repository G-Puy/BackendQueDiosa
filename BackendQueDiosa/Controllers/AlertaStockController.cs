using DTOS;
using IRepositorios;
using Microsoft.AspNetCore.Mvc;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertaStockController : ControllerBase
    {

        private IRepositorioAlertaStock ManejadorAlerta { get; set; }

        public AlertaStockController([FromServices] IRepositorioAlertaStock repInj)
        {
            this.ManejadorAlerta = repInj;
        }

        [HttpGet("Leida")]
        public IActionResult Leida(long idAlerta)
        {
            try
            {
                if (ManejadorAlerta.Leer(idAlerta)) return Ok(true);
                else return BadRequest(false);
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
                List<DTOAlertaStock> resultado = (List<DTOAlertaStock>)this.ManejadorAlerta.TraerTodos();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Contar")]
        public IActionResult Contar(long idAlerta)
        {
            try
            {
                int cantidad = this.ManejadorAlerta.Contar(idAlerta);
                if (cantidad < 0) return BadRequest(cantidad);
                else return Ok(cantidad);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
