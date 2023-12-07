using Dominio.Entidades;
using DTOS;
using DTOS.DTOSProductoFrontBack;
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
        public IActionResult Modificar([FromBody] DTOStockEnvio dtoStockEnvio)
        {
            try
            {
                List<DTOStock> stocks = new List<DTOStock>();
                foreach (DTOTalleEnvio talle in dtoStockEnvio.Talles)
                {
                    foreach (DTOColorEnvio color in talle.Colores)
                    {
                        DTOStock dtoStock = new DTOStock();
                        dtoStock.Id = dtoStockEnvio.Id;
                        dtoStock.IdProducto = dtoStockEnvio.IdProducto;
                        dtoStock.IdColor = color.Id;
                        dtoStock.IdTalle = talle.Id;
                        dtoStock.Cantidad = color.Cantidad;

                        stocks.Add(dtoStock);
                    }
                }


                bool resultado = this.ManejadorStock.Modificar(stocks);

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
