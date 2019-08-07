using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Data;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Documents.GitHub.DatingAPP.DatingApp.API.DTO;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegistrationDTO userForRegistrationDTO)
        {
            userForRegistrationDTO.Username = userForRegistrationDTO.Username.ToLower();

            //Verifica se o usuario existe no Banco
            if (await _repo.UserExists(userForRegistrationDTO.Username))
            {
                return BadRequest("Usuario Cadastrado");
            }

            //Passa o parametro recebido do usuario para o objeto do Model    
            var UserToModel = new User
            {
                UserName = userForRegistrationDTO.Username
            };

            //Grava o objeto do Model no Banco de Dados
            var UserToBD = await _repo.Register(UserToModel, userForRegistrationDTO.Password);
            return StatusCode(201);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var userFromRepo = await _repo.Login(userForLoginDTO.Username.ToLower(), userForLoginDTO.Password);

            if (userFromRepo == null)
                return Unauthorized();  
        

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserName)
            };

            //Criar a Chave
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            //encripta a chave em uma credencial
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);    

            //criar a descricao do token de seguranca
            var tokenDescriptor = new SecurityTokenDescriptor 
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),   
                SigningCredentials = credentials        
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });

        }


    }
}