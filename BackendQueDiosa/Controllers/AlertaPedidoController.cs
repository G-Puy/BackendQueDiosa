﻿using DTOS;
using DTOS.DTOSProductoFrontBack;
using IRepositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace BackendQueDiosa.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AlertaPedidoController : ControllerBase
    {

        private IRepositorioPedido ManejadorPedido { get; set; }

        public AlertaPedidoController([FromServices] IRepositorioPedido repInj)
        {
            this.ManejadorPedido = repInj;
        }

        [HttpGet("Realizado")]
        public IActionResult Realizado(long idAlerta)
        {
            try
            {
                if (ManejadorPedido.Realizado(idAlerta)) return Ok(true);
                else return BadRequest(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("TraerFiltrados")]
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

        [HttpGet("Contar")]
        public IActionResult Contar()
        {
            try
            {
                int cantidad = this.ManejadorPedido.Contar();
                if (cantidad < 0) return BadRequest(cantidad);
                else return Ok(cantidad);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
