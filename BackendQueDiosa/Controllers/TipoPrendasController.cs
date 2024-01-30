using BackendQueDiosa.Mappers;
using DTOS;
using IRepositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoPrendasController : ControllerBase
    {

        private IRepositorioTipoPrenda ManejadorTipoPrenda { get; set; }

        public TipoPrendasController([FromServices] IRepositorioTipoPrenda repInj)
        {

            this.ManejadorTipoPrenda = repInj;
        }

        [Authorize]
        [HttpPost("alta")]
        public IActionResult Alta([FromBody] DTOTipoPrenda dtoTp)
        {
            try
            {
                if (this.ManejadorTipoPrenda.NombreOcupado(dtoTp)) return BadRequest("Nombre ya existe");

                bool resultado = this.ManejadorTipoPrenda.Alta(dtoTp);

                if (resultado) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Authorize]
        [HttpPost("bajaLogica")]
        public IActionResult BajaLogica([FromBody] DTOTipoPrenda dtoTipoPrenda)
        {
            try
            {
                bool resultado = this.ManejadorTipoPrenda.BajaLogica(dtoTipoPrenda);

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
                DTOTipoPrenda dtoTipoPrenda = new DTOTipoPrenda();
                dtoTipoPrenda.Id = id;

                DTOTipoPrenda resultado = this.ManejadorTipoPrenda.BuscarPorId(dtoTipoPrenda);

                if (resultado != null && resultado.Id > 0) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("buscarPorNombre")]
        public IActionResult BuscarPorNombreDePrenda(string nombreDePrenda)
        {
            try
            {
                DTOTipoPrenda dtoTipoPrenda = new DTOTipoPrenda();
                dtoTipoPrenda.Nombre = nombreDePrenda;

                DTOTipoPrenda resultado = this.ManejadorTipoPrenda.BuscarPorNombre(dtoTipoPrenda);

                if (resultado != null && resultado.Id > 0) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        [HttpDelete("eliminar")]
        public IActionResult Eliminar(long idTipoPrenda)
        {
            try
            {
                DTOTipoPrenda dtoTipoPrenda = new DTOTipoPrenda();
                dtoTipoPrenda.Id = idTipoPrenda;

                bool resultado = false;

                if (this.ManejadorTipoPrenda.EnUso(dtoTipoPrenda))
                {
                    return BadRequest("En uso.");
                }
                else
                {
                    resultado = this.ManejadorTipoPrenda.Eliminar(dtoTipoPrenda);
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
        public IActionResult Modificar([FromBody] DTOTipoPrenda dtoCat)
        {
            try
            {
                if (this.ManejadorTipoPrenda.NombreOcupado(dtoCat))
                    return BadRequest("Ya existe nombre");

                bool resultadoEditar = this.ManejadorTipoPrenda.Modificar(dtoCat);

                if (resultadoEditar) return Ok(true);
                else return BadRequest("Fallo al modificar");

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("traerTodos")]
        public IActionResult TraerTodos()
        {
            try
            {
                List<DTOTipoPrenda> resultado = (List<DTOTipoPrenda>)this.ManejadorTipoPrenda.TraerTodos();
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
