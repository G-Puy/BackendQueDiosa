using BackendQueDiosa.Mappers;
using DTOS;
using IRepositorios;
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



        [HttpPost("altaTipoPrenda")]
        public IActionResult Alta([FromBody] DTOTipoPrenda dtoTp)
        {
            try
            {
                bool resultado = this.ManejadorTipoPrenda.Alta(dtoTp);

                if (this.ManejadorTipoPrenda.BuscarPorNombre(dtoTp) == null) return BadRequest("Nombre ya existe");

                if (resultado) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost("bajaLogica")]
        public IActionResult BajaLogica([FromBody] MapperTipoPrenda mapperTipoPrenda)
        {
            try
            {
                DTOTipoPrenda dtoTipoPrenda = new DTOTipoPrenda();
                dtoTipoPrenda.Id = mapperTipoPrenda.Id;
                dtoTipoPrenda.Nombre = mapperTipoPrenda.Nombre;
                dtoTipoPrenda.BajaLogica = mapperTipoPrenda.BajaLogica;

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

                if (!(resultado.Id == null)) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("buscarPorNombreDePrenda")]
        public IActionResult BuscarPorNombreDePrenda(string nombreDePrenda)
        {
            try
            {
                DTOTipoPrenda dtoTipoPrenda = new DTOTipoPrenda();
                dtoTipoPrenda.Nombre = nombreDePrenda;

                DTOTipoPrenda resultado = this.ManejadorTipoPrenda.BuscarPorNombre(dtoTipoPrenda);

                if (!(resultado.Id == null)) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        [HttpDelete("eliminarTipoPrenda")]
        public IActionResult Eliminar(long idTipoPrenda)
        {
            try
            {
                DTOTipoPrenda dtoTipoPrenda = new DTOTipoPrenda();
                dtoTipoPrenda.Id = idTipoPrenda;

                //if (!this.ManejadorTipoPrenda.EnUso(dtoTipoPrenda)) return BadRequest("En uso");

                bool resultadoEliminar = this.ManejadorTipoPrenda.Eliminar(dtoTipoPrenda);


                if (resultadoEliminar) return Ok(resultadoEliminar);
                else return BadRequest(resultadoEliminar);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPut("editarTipoPrenda")]
        public IActionResult Modificar([FromBody] MapperTipoPrenda mapperCategoriaFront)
        {
            try
            {
                DTOTipoPrenda dtoCat = new DTOTipoPrenda();
                dtoCat.Nombre = mapperCategoriaFront.Nombre;
                dtoCat.Id = mapperCategoriaFront.Id;

                bool resultadoEditar = this.ManejadorTipoPrenda.Modificar(dtoCat);

                if (resultadoEditar) return Ok(resultadoEditar);
                else return BadRequest(resultadoEditar);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("TraerTiposPrenda")]
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
