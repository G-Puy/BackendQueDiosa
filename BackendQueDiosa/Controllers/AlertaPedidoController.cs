﻿using DTOS;
using DTOS.DTOSProductoFrontBack;
using IRepositorios;
using Microsoft.AspNetCore.Mvc;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertaPedidoController : ControllerBase
    {

        private IRepositorioPedido ManejadorPedido { get; set; }

        public AlertaPedidoController([FromServices] IRepositorioPedido repInj)
        {
            this.ManejadorPedido = repInj;
        }

        [HttpGet("Entregado")]
        public IActionResult Entregado(long idVenta)
        {
            try
            {
                if (ManejadorPedido.Entregado(idVenta)) return Ok(true);
                else return BadRequest(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("TraerFiltrados")]
        public IActionResult TraerFiltrados([FromBody] DTOFiltroAlertasPedidos dto)
        {
            try
            {
                var resultado = ManejadorPedido.TraerFiltrado(dto);
                return Ok(resultado);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }


        [HttpGet("TraerTodos")]
        public IActionResult TraerTodos()
        {
            try
            {
                List<DTOAlertaPedido> resultado = (List<DTOAlertaPedido>)this.ManejadorPedido.TraerTodos();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
