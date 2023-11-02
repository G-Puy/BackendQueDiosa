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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private IRepositorioLogin ManejadorLogin { get; set; }
        private IConfiguration config;

        public UsuarioController([FromServices] IRepositorioLogin repInj, IConfiguration config)
        {

            this.ManejadorLogin = repInj;
            this.config = config;
        }

        [HttpPost("altaUsuario")]
        public IActionResult Alta([FromBody] MapperUsuario mapperUsuario)
        {

            try
            {
                DTOUsuario dtoUsuario = new DTOUsuario();
                dtoUsuario.NombreDeUsuario = mapperUsuario.NombreDeUsuario;
                dtoUsuario.Nombre = mapperUsuario.Nombre;
                dtoUsuario.Apellido = mapperUsuario.Apellido;
                dtoUsuario.Contraseña = mapperUsuario.Contraseña;
                dtoUsuario.Correo = mapperUsuario.Correo;
                dtoUsuario.Telefono = mapperUsuario.Telefono;
                dtoUsuario.TipoUsuario = mapperUsuario.TipoUsuario;

                bool resultadoAlta = this.ManejadorLogin.Alta(dtoUsuario);

                if (resultadoAlta) return Ok(resultadoAlta);
                else return BadRequest(resultadoAlta);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        [HttpPost("loginUsuario")]
        public IActionResult Login([FromBody] MapperUsuarioLogin mapperUsuarioLogin)
        {
            try
            {
                DTOUsuario dtoUsuario = new DTOUsuario();
                dtoUsuario.NombreDeUsuario = mapperUsuarioLogin.NombreDeUsuario;
                dtoUsuario.Contraseña = mapperUsuarioLogin.Contraseña;

                bool resultadoLogin = this.ManejadorLogin.Login(dtoUsuario);

                if (resultadoLogin) { 
                    string jwtToken = GenerateToken(dtoUsuario);
                    return Ok(resultadoLogin);}
                else return BadRequest(resultadoLogin);
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
                new Claim(ClaimTypes.Name, dtoUsuario.Nombre),
                new Claim(ClaimTypes.Email, dtoUsuario.Correo)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(60), signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }
    }
}
