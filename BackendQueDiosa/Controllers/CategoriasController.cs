using BackendQueDiosa.Mappers;
using DTOS;
using IRepositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private IRepositorioCategoria ManejadorCategoria { get; set; }

        public CategoriasController([FromServices] IRepositorioCategoria repInj)
        {

            this.ManejadorCategoria = repInj;
        }



        [HttpPost("altaCategoria")]
        public void AltaCategoria([FromBody] MapperCategoria mapperCategoriaFront)
        {

            try
            {
                DTOCategoria dtoCat = new DTOCategoria();
                dtoCat.NombreCategoria = mapperCategoriaFront.NombreCategoria;
                this.ManejadorCategoria.Alta(dtoCat);
            }
            catch (Exception ex)
            {

                throw ex;
            }




        }


    }
}
