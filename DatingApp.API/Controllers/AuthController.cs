using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Data;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Documents.GitHub.DatingAPP.DatingApp.API.DTO;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        public AuthController(IAuthRepository repo)
        {
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
            var UserToModel = new User{
                UserName = userForRegistrationDTO.Username
            };

            //Grava o objeto do Model no Banco de Dados
            var UserToBD = await _repo.Register(UserToModel, userForRegistrationDTO.Password);
            return StatusCode(201);

        }
    }
}