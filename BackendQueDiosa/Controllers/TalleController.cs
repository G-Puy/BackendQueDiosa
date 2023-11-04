using BackendQueDiosa.Mappers;
using DTOS;
using IRepositorios;
using Microsoft.AspNetCore.Mvc;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TalleController : ControllerBase
    {
        private IRepositorioTalle ManejadorTalle { get; set; }

        public TalleController([FromServices] IRepositorioTalle repInj)
        {
            this.ManejadorTalle = repInj;
        }

        [HttpPost("altaTalle")]
        public IActionResult AltaTalle([FromBody] MapperTalle mapperTalleFront)
        {
            try
            {
                DTOTalle dtoTal = new DTOTalle();
                dtoTal.Nombre = mapperTalleFront.Nombre;

                bool resultadoAlta = this.ManejadorTalle.Alta(dtoTal);

                if (resultadoAlta) return Ok(resultadoAlta);
                else return BadRequest(resultadoAlta);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("eliminarTalle")]
        public IActionResult EliminarTalle(long idTalle)
        {
            try
            {
                DTOTalle dtoTal = new DTOTalle();
                dtoTal.IdTalle = idTalle;
                bool resultadoEliminar = this.ManejadorTalle.Eliminar(dtoTal);
                if (resultadoEliminar) return Ok(resultadoEliminar);
                else return BadRequest(resultadoEliminar);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("editarTalle")]
        public IActionResult EditarTalle([FromBody] MapperColor mapperTalleFront)
        {
            try
            {
                DTOTalle dtoTal = new DTOTalle();
                dtoTal.Nombre = mapperTalleFront.Nombre;
                dtoTal.IdTalle = mapperTalleFront.IdColor;

                bool resultadoEditar = this.ManejadorTalle.Modificar(dtoTal);

                if (resultadoEditar) return Ok(resultadoEditar);
                else return BadRequest(resultadoEditar);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("TraerTalles")]
        public IActionResult TraerTalles()
        {
            try
            {
                List<DTOTalle> resultado = (List<DTOTalle>)this.ManejadorTalle.TraerTodos();
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
