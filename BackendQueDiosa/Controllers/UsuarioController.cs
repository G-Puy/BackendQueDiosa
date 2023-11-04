﻿using BackendQueDiosa.Mappers;
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

        private IRepositorioLogin ManejadorLogin { get; set; }
        private IConfiguration config;

        public UsuarioController([FromServices] IRepositorioLogin repInj, IConfiguration config)
        {

            this.ManejadorLogin = repInj;
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
                dtoUsuario.Contraseña = mapperUsuario.Contraseña;
                dtoUsuario.Correo = mapperUsuario.Correo;
                dtoUsuario.Telefono = mapperUsuario.Telefono;
                dtoUsuario.TipoUsuario = mapperUsuario.IdTipoUsuario;

                bool resultadoAlta = this.ManejadorLogin.Alta(dtoUsuario);

                if (resultadoAlta) return Ok(resultadoAlta);
                else return BadRequest(resultadoAlta);

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
                bool resultadoLogin = this.ManejadorLogin.Login(dtoUsuarioFront);
                if (resultadoLogin) { 
                    string jwtToken = GenerateToken(dtoUsuarioFront);
                    return Ok(jwtToken);}
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
    }
}
