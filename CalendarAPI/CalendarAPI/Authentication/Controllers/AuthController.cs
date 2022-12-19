using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CalendarAPI.Authentication.Model;
using CalendarAPI.Authentication.Models.Requests;
using CalendarAPI.Authentication.Repositories;
using CalendarAPI.Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CalendarAPI.Authentication.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromServices] UserRepository userRepository,
                                      [FromBody] NewUser newuser)
        {
            if (!ModelState.IsValid)
                return BadRequest("Body inválido");

            try
            {
                User usu = new User()
                {
                    Id = 0,
                    Name = newuser.name,
                    Email = newuser.email,
                    Password = newuser.password
                };

                var insertedUsu = await userRepository.CadastroDeUserAsync(usu);
                if (usu.Id == 0)
                {
                    return UnprocessableEntity(
                       new
                       {
                           status = HttpStatusCode.UnprocessableEntity,
                           Error = "Não foi possível adicionar usuário"
                       }
                    );
                }
                else
                {
                    return Created($"New User:", insertedUsu);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromServices] TokenService _tokenService,
                                  [FromServices] UserRepository userRepository,
                                  [FromBody] LoginUser loginUser)
        {

            if (!ModelState.IsValid)
                return BadRequest("Body inválido");

            try
            {
                User usu = new User()
                {
                    Id = 0,
                    Name = null,
                    Email = loginUser.Email,
                    Password = loginUser.Password
                };

                var usuConsulta = await userRepository.VerificarUsuarioSenhaAsync(usu);

                if (usuConsulta != null)
                {
                    if (usuConsulta.Id != 0)
                    {
                        var token = await _tokenService.GenerateToken(usuConsulta);
                        return Ok(
                            new
                            {
                                status = HttpStatusCode.OK,
                                Token = token
                            });
                    }
                    else
                    {
                        return Unauthorized(
                           new
                           {
                               status = HttpStatusCode.Unauthorized,
                               Error = "Não autorizado"
                           });
                    }
                }
                else
                {
                    return NotFound(
                        new
                        {
                            status = HttpStatusCode.NotFound,
                            Error = "Não encontrado"
                        });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Falha ao realizar login.");
            }
        }

        //[Authorize]
        //[HttpDelete("delete/{id}")]
        //public async Task<IActionResult> DeleteUserAsync([FromServices] NE_User neUser, int id)
        //{
        //    try
        //    {
        //        var sucess = await neUser.DeleteUserAsync(id);

        //        if (!sucess)
        //        {
        //            return NotFound(
        //               new
        //               {
        //                   status = HttpStatusCode.NotFound,
        //                   Error = Notificacoes()
        //               });
        //        }
        //        else
        //        {
        //            return Ok();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Notificar("Falha ao deletar usuário.");
        //        return StatusCode(500, Notificacoes());
        //    }
        //}

        //[AllowAnonymous]
        //[HttpPatch("alterarSenha")]
        //public async Task<IActionResult> AlterarSenhaAsync([FromServices] NE_User neUser, [FromBody] AlterarUserSenhaViewModel viewModel)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new ResultViewModel<User>(ModelState.RecuperarErros()));

        //    try
        //    {
        //        User usu = new User()
        //        {
        //            user_id = 0,
        //            user_nome = viewModel.user_nome,
        //            user_email = viewModel.user_email,
        //            user_accessKey = viewModel.user_accessKey
        //        };

        //        var sucess = await neUser.alterarSenhaAsync(usu);

        //        if (!sucess)
        //        {
        //            return NotFound(
        //               new
        //               {
        //                   status = HttpStatusCode.NotFound,
        //                   Error = Notificacoes()
        //               });
        //        }
        //        else
        //        {
        //            return Ok();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Notificar("Falha ao alterar senha do usuário.");
        //        return BadRequest(Notificacoes());
        //    }
        //}


    }

}

