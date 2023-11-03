using BackendQueDiosa.Mappers;
using DTOS;
using IRepositorios;
using Microsoft.AspNetCore.Mvc;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {

        private IRepositorioColor ManejadorColor { get; set; }

        public ColorController([FromServices] IRepositorioColor repInj)
        {

            this.ManejadorColor = repInj;
        }



        [HttpPost("altaColor")]
        public IActionResult AltaColor ([FromBody] MapperColor mapperColorFront)
        {
            try
            {
                DTOColor dtoCol = new DTOColor ();
                dtoCol.Nombre = mapperColorFront.Nombre;

                bool resultadoAlta = this.ManejadorColor.Alta(dtoCol);

                if (resultadoAlta) return Ok(resultadoAlta);
                else return BadRequest(resultadoAlta);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpDelete("eliminarColor")]
        public IActionResult EliminarColor(long idColor)
        {
            try
            {
                DTOColor dtoCol = new DTOColor ();
                dtoCol.IdColor = idColor;
                bool resultadoEliminar = this.ManejadorColor.Eliminar(dtoCol);
                if (resultadoEliminar) return Ok(resultadoEliminar);
                else return BadRequest(resultadoEliminar);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPut("editarColor")]
        public IActionResult EditarColor([FromBody] MapperColor mapperColorFront)
        {
            try
            {
                DTOColor dtoCol = new DTOColor();
                dtoCol.Nombre = mapperColorFront.Nombre;
                dtoCol.IdColor = mapperColorFront.IdColor;

                bool resultadoEditar = this.ManejadorColor.Modificar(dtoCol);

                if (resultadoEditar) return Ok(resultadoEditar);
                else return BadRequest(resultadoEditar);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("TraerColores")]
        public IActionResult TraerColores()
        {
            try
            {
                List<DTOColor> resultado = (List<DTOColor>)this.ManejadorColor.TraerTodos();
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

