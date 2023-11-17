using BackendQueDiosa.Mappers;
using DTOS;
using IRepositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

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

        [Authorize]
        [HttpPost("alta")]
        public IActionResult Alta([FromBody] DTOColor dtoCol)
        {
            try
            {
                if (this.ManejadorColor.BuscarPorNombre(dtoCol) != null) return BadRequest("Nombre ya existe");

                bool resultadoAlta = this.ManejadorColor.Alta(dtoCol);

                if (resultadoAlta) return Ok(resultadoAlta);
                else return BadRequest(resultadoAlta);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost("bajaLogica")]
        public IActionResult BajaLogica([FromBody] DTOColor dtoColor)
        {
            try
            {
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

                if (resultado != null && resultado.Id > 0) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("buscarPorNombre")]
        public IActionResult BuscarPorNombre(string nombreDeColor)
        {
            try
            {
                DTOColor dtoColor = new DTOColor();
                dtoColor.Nombre = nombreDeColor;

                DTOColor resultado = this.ManejadorColor.BuscarPorNombre(dtoColor);

                if (resultado != null && resultado.Id > 0) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpDelete("eliminar")]
        public IActionResult Eliminar(long idColor)
        {
            try
            {
                DTOColor dtoCol = new DTOColor();
                dtoCol.Id = idColor;
                bool resultado = false;

                if (this.ManejadorColor.EnUso(dtoCol))
                {
                    resultado = this.ManejadorColor.BajaLogica(dtoCol);
                }
                else
                {
                    resultado = this.ManejadorColor.Eliminar(dtoCol);
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
        public IActionResult Modificar([FromBody] DTOColor dtoCol)
        {
            try
            {
                if (this.ManejadorColor.BuscarPorNombre(dtoCol) != null)
                    return BadRequest("Ya existe nombre");

                bool resultado = this.ManejadorColor.Modificar(dtoCol);


                if (resultado) return Ok("Modificado exitosamente");
                else return BadRequest("Fallo al modificar");

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

