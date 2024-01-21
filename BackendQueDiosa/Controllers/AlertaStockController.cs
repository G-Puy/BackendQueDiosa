﻿using DTOS;
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
        public IActionResult Leida(long idVenta)
        {
            try
            {
                if (ManejadorAlerta.Leer(idVenta)) return Ok(true);
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
    }
}
