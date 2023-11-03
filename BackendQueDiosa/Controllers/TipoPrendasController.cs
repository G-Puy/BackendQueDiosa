using BackendQueDiosa.Mappers;
using Conexiones;
using DTOS;
using IRepositorios;
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



        [HttpPost("altaTipoProducto")]
        public IActionResult AltaTipoProducto([FromBody] MapperTipoPrenda mapperCategoriaFront)
        {
            try
            {
                DTOTipoPrenda dtoCat = new DTOTipoPrenda();
                dtoCat.NombreTipoPrenda = mapperCategoriaFront.NombreCategoria;

                bool resultadoAlta = this.ManejadorCategoria.Alta(dtoCat);

                if (resultadoAlta) return Ok(resultadoAlta);
                else  return BadRequest(resultadoAlta);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpDelete("eliminarTipoProducto")]
        public IActionResult EliminarTipoProducto(long idTipoPrenda)
        {
            try
            {
                DTOTipoPrenda dtoCat = new DTOTipoPrenda();
                dtoCat.IdTipoPrenda = idTipoPrenda;
                bool resultadoEliminar = this.ManejadorCategoria.Eliminar(dtoCat);
                if (resultadoEliminar) return Ok(resultadoEliminar);
                else return BadRequest(resultadoEliminar);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
