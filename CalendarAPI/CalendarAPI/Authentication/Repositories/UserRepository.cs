using System;
using CalendarAPI.Authentication.Model;
using CalendarAPI.Authentication.Models.Requests;
using CalendarAPI.Authentication.Util;
using CalendarAPI.Database;

namespace CalendarAPI.Authentication.Repositories
{
	public class UserRepository
	{
        private readonly IConfiguration _configuration;

        public async Task<User?> GetUserByEmail(String userMail)
        {
            try
            {
                using (var ctx = new CalendarDBContext())
                {
                    User existUser = ctx.Users.Where(w=>w.Email ==userMail).FirstOrDefault();

                    return existUser;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<object> CadastroDeUserAsync(User newUser)
        {
            try
            {
                var existUser = await GetUserByEmail(newUser.Email);

                if (existUser == null)
                {
                    newUser.Password = Criptography.HashValue(newUser.Password);

                    using (var ctx = new CalendarDBContext())
                    {
                        ctx.Users.Add(newUser);
                        ctx.SaveChanges();
                        existUser = await GetUserByEmail(newUser.Email);
                        return new
                            {
                                Id = existUser.Id,
                                Email = existUser.Email,
                                Name = existUser.Name
                            };
                    }
                }
                else
                    return existUser;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<User> VerificarUsuarioSenhaAsync(User usu)
        {
            try
            {
                var usuConsulta = await GetUserByEmail(usu.Email);
                if (usuConsulta != null)
                {
                    usu.Password = Criptography.HashValue(usu.Password);


                    if (usuConsulta.Password == usu.Password)
                        return usuConsulta;
                    else
                    {
                        usu.Id = 0;
                        throw new Exception("Senha não compatível");
                    }
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível verificar senha do usuário.");
            }
        }
    }
}

