using BackendQueDiosa.Mappers;
using DTOS;
using IRepositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendQueDiosa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private IRepositorioUsuario ManejadorUsuario;
        private IConfiguration config;

        public UsuarioController([FromServices] IRepositorioUsuario repInj, IConfiguration config)
        {

            this.ManejadorUsuario = repInj;
            this.config = config;
        }

        [Authorize]
        [HttpPost("alta")]
        public IActionResult Alta([FromBody] MapperUsuarioAlta mapperUsuario)
        {

            try
            {
                DTOUsuario dtoUsuario = new DTOUsuario();
                dtoUsuario.NombreDeUsuario = mapperUsuario.NombreDeUsuario;
                dtoUsuario.Nombre = mapperUsuario.Nombre;
                dtoUsuario.Apellido = mapperUsuario.Apellido;
                dtoUsuario.Contrasenia = mapperUsuario.Contraseña;
                dtoUsuario.Correo = mapperUsuario.Correo;
                dtoUsuario.Telefono = mapperUsuario.Telefono;
                dtoUsuario.TipoUsuario = mapperUsuario.IdTipoUsuario;

                bool resultado = this.ManejadorUsuario.Alta(dtoUsuario);

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
        public IActionResult BajaLogica([FromBody] MapperUsuarioBajaLogica mapperUsuario)
        {
            try
            {
                DTOUsuario dtoUsuario = new DTOUsuario();
                dtoUsuario.IdUsuario = mapperUsuario.IdUsuario;
                dtoUsuario.BajaLogica = mapperUsuario.BajaLogica;

                bool resultado = this.ManejadorUsuario.BajaLogica(dtoUsuario);

                if (resultado) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("buscarPorId")]
        public IActionResult BuscarPorId([FromBody] MapperUsuarioId mapperUsuario)
        {
            try
            {
                DTOUsuario dtoUsuario = new DTOUsuario();
                dtoUsuario.IdUsuario = mapperUsuario.IdUsuario;

                DTOUsuario resultado = this.ManejadorUsuario.BuscarPorId(dtoUsuario);

                if (!(resultado.IdUsuario == null)) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("buscarPorNombreDeUsuario")]
        public IActionResult BuscarPorNombreDeUsuario([FromBody] MapperUsuarioNombre mapperUsuario)
        {
            try
            {
                DTOUsuario dtoUsuario = new DTOUsuario();
                dtoUsuario.NombreDeUsuario = mapperUsuario.NombreDeUsuario;

                DTOUsuario resultado = this.ManejadorUsuario.BuscarPorNombreDeUsuario(dtoUsuario);

                if (!(resultado.IdUsuario == null)) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost("eliminar")]
        public IActionResult Eliminar([FromBody] MapperUsuarioId mapperUsuario)
        {
            try
            {
                DTOUsuario dtoUsuario = new DTOUsuario();
                dtoUsuario.IdUsuario = mapperUsuario.IdUsuario;

                bool resultado = this.ManejadorUsuario.Eliminar(dtoUsuario);

                if (resultado) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] DTOUsuario dtoUsuarioFront)
        {
            try
            {
                DTOUsuario resultadoLogin = this.ManejadorUsuario.Login(dtoUsuarioFront);
                if (resultadoLogin != null)
                {
                    string jwtToken = GenerateToken(dtoUsuarioFront);
                    resultadoLogin.Contrasenia = jwtToken;
                    return Ok(resultadoLogin);
                }
                else return BadRequest();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateToken(DTOUsuario dtoUsuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dtoUsuario.NombreDeUsuario)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
            string nnombre = config.GetSection("JWT:Key").Value;
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }

        [Authorize]
        [HttpPost("modificar")]
        public IActionResult Modificar([FromBody] DTOUsuario dtoUsuario)
        {
            try
            {
                bool resultado = this.ManejadorUsuario.Eliminar(dtoUsuario);

                if (resultado) return Ok(resultado);
                else return BadRequest(resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("traerTodos")]
        public IActionResult TraerTodos()
        {
            try
            {
                List<DTOUsuario> resultado = (List<DTOUsuario>)this.ManejadorUsuario.TraerTodos();
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
