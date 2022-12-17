using System;
using System.ComponentModel.DataAnnotations;

namespace CalendarAPI.Authentication.Models.Requests
{
	public class NewUser
	{
        [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
        [StringLength(60, ErrorMessage = "Nome de usuário pode ter no máximo 60 caracteres.")]
        public string name { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido...")]
        [StringLength(100, ErrorMessage = "E-mail deve ter no máximo 100 caracteres.")]
        public string email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Senha deve ter de 6 a 10 caracteres.")]
        public string password { get; set; }
    }
}

