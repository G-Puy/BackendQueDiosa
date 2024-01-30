using BackendQueDiosa.Mappers;
using DTOS;
using IRepositorios;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost("alta")]
        public IActionResult Alta([FromBody] DTOTalle dtoTal)
        {
            try
            {

                if (this.ManejadorTalle.NombreOcupado(dtoTal)) return BadRequest("Nombre ya existe");
                dtoTal.Nombre = dtoTal.Nombre.ToUpper();
                bool resultadoAlta = this.ManejadorTalle.Alta(dtoTal);

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
        public IActionResult BajaLogica([FromBody] DTOTalle dtoTalle)
        {
            try
            {
                bool resultado = this.ManejadorTalle.BajaLogica(dtoTalle);

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
                DTOTalle dtoTalle = new DTOTalle();
                dtoTalle.Id = id;

                DTOTalle resultado = this.ManejadorTalle.BuscarPorId(dtoTalle);

                if (resultado != null && resultado.Id > 0) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("buscarPorNombre")]
        public IActionResult BuscarPorNombre(string nombreDeTalle)
        {
            try
            {
                DTOTalle dtoTalle = new DTOTalle();
                dtoTalle.Nombre = nombreDeTalle;

                DTOTalle resultado = this.ManejadorTalle.BuscarPorNombre(dtoTalle);

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
        public IActionResult Eliminar(long idTalle)
        {
            try
            {
                DTOTalle dtoTal = new DTOTalle();
                dtoTal.Id = idTalle;
                bool resultado = false;

                if (this.ManejadorTalle.EnUso(dtoTal))
                {
                    resultado = this.ManejadorTalle.BajaLogica(dtoTal);
                }
                else
                {
                    resultado = this.ManejadorTalle.Eliminar(dtoTal);
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
        public IActionResult Modificar([FromBody] DTOTalle dtoTal)
        {
            try
            {
                    dtoTal.Nombre = dtoTal.Nombre.ToUpper();
                if (this.ManejadorTalle.NombreOcupado(dtoTal))
                    return BadRequest("Ya existe nombre");

                bool resultado = this.ManejadorTalle.Modificar(dtoTal);

                if (resultado) return Ok(true);
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
