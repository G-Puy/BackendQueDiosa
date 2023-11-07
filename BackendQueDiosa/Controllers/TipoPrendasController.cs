using BackendQueDiosa.Mappers;
using Conexiones;
using DTOS;
using IRepositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoPrendasController : ControllerBase
    {

        private IRepositorioTipoPrenda ManejadorCategoria { get; set; }

        public TipoPrendasController([FromServices] IRepositorioTipoPrenda repInj)
        {

            this.ManejadorCategoria = repInj;
        }


        
        [HttpPost("altaTipoPrenda")]
        public IActionResult AltaTipoPrenda([FromBody] DTOTipoPrenda dtoTp)
        {
            try
            {
                bool resultadoAlta = this.ManejadorCategoria.Alta(dtoTp);
                if (resultadoAlta) return Ok(resultadoAlta);
                else return BadRequest(resultadoAlta);

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
                dtoTipoPrenda.Id = mapperTipoPrenda.IdTipoProducto;
                dtoTipoPrenda.Nombre = mapperTipoPrenda.NombreTipoProducto;
                dtoTipoPrenda.BajaLogica = mapperTipoPrenda.BajaLogica;

                bool resultado = this.ManejadorCategoria.BajaLogica(dtoTipoPrenda);

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

                DTOTipoPrenda resultado = this.ManejadorCategoria.BuscarPorId(dtoTipoPrenda);

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

                DTOTipoPrenda resultado = this.ManejadorCategoria.BuscarPorNombreDePrenda(dtoTipoPrenda);

                if (!(resultado.Id == null)) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        
        [HttpDelete("eliminarTipoPrenda")]
        public IActionResult EliminarTipoProducto(long idTipoPrenda)
        {
            try
            {
                DTOTipoPrenda dtoCat = new DTOTipoPrenda();
                dtoCat.Id = idTipoPrenda;
                bool resultadoEliminar = this.ManejadorCategoria.Eliminar(dtoCat);
                if (resultadoEliminar) return Ok(resultadoEliminar);
                else return BadRequest(resultadoEliminar);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPut("editarTipoPrenda")]
        public IActionResult EditarTipoProducto([FromBody] MapperTipoPrenda mapperCategoriaFront)
        {
            try
            {
                DTOTipoPrenda dtoCat = new DTOTipoPrenda();
                dtoCat.Nombre = mapperCategoriaFront.NombreTipoProducto;
                dtoCat.Id = mapperCategoriaFront.IdTipoProducto;

                bool resultadoEditar = this.ManejadorCategoria.Modificar(dtoCat);

                if (resultadoEditar) return Ok(resultadoEditar);
                else return BadRequest(resultadoEditar);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("TraerTiposPrenda")]
        public IActionResult TraerTiposProductos()
        {
            try
            {
                List<DTOTipoPrenda> resultado = (List<DTOTipoPrenda>)this.ManejadorCategoria.TraerTodos();
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
