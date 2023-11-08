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

        [HttpPost("alta")]
        public IActionResult AltaColor ([FromBody] MapperColor mapperColorFront)
        {
            try
            {
                DTOColor dtoCol = new DTOColor();
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

        [HttpPost("bajaLogica")]
        public IActionResult BajaLogica([FromBody] MapperColor mapperColor)
        {
            try
            {
                DTOColor dtoColor = new DTOColor();
                dtoColor.Id = mapperColor.Id;
                dtoColor.Nombre = mapperColor.Nombre;
                dtoColor.BajaLogica = mapperColor.BajaLogica;

                bool resultado = this.ManejadorColor.BajaLogica(dtoColor);

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
                DTOColor dtoColor = new DTOColor();
                dtoColor.Id = id;

                DTOColor resultado = this.ManejadorColor.BuscarPorId(dtoColor);

                if (!(resultado.Id == null)) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("buscarPorNombreDeColor")]
        public IActionResult BuscarPorNombreDeColor(string nombreDeColor)
        {
            try
            {
                DTOColor dtoColor = new DTOColor();
                dtoColor.Nombre = nombreDeColor;

                DTOColor resultado = this.ManejadorColor.BuscarPorNombreDeColor(dtoColor);

                if (!(resultado.Id == null)) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("eliminar")]
        public IActionResult EliminarColor(long idColor)
        {
            try
            {
                DTOColor dtoCol = new DTOColor();
                dtoCol.Id = idColor;
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
                dtoCol.Id = mapperColorFront.Id;

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
                if (resultado.Count>0) return Ok(resultado);
                else return BadRequest(false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

