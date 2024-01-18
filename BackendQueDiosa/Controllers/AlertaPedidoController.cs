using DTOS;
using IRepositorios;
using Microsoft.AspNetCore.Mvc;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertaPedidoController: ControllerBase
    {

        private IRepositorioPedido ManejadorPedido { get; set; }

        public AlertaPedidoController([FromServices] IRepositorioPedido repInj)
        {
            this.ManejadorPedido = repInj;
        }

        [HttpPost("Entregado")]
        public IActionResult Entregado([FromHeader] long id)
        {
            try
            {
                if (ManejadorPedido.Entregado(id)) return Ok(true);
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
                List<DTOAlertaStock> resultado = (List<DTOAlertaStock>)this.ManejadorPedido.TraerTodos();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
