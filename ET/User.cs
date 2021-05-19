using System;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public int RoleID { get; set; }

        public string RoleName { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool EmailValidated { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool NeedResetPwd { get; set; }

        public bool Subscriber { get; set; }

        public bool ActiveFlag { get; set; }

        public DateTime LastActivityDate { get; set; }

        public string Token { get; set; }

        public DateTime TokenExpires { get; set; }

        public int TokenExpiresMin { get; set; }

        public DateTime CreationDate { get; set; }

        public string ActionType { get; set; }

    }

    public class UserLogin
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string IP { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }
    }
    public class AuthorizationCode
    {
        public string GUID { get; set; }

        public int UserID { get; set; }

        public string FullName { get; set; }
    }

    public class ResetPasswordModel
    {

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{6,20}$", ErrorMessage = "Contraseña invalida, debe tener entre 6 - 20 caracteres y contener al menos un número y una mayúscula")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        public string GUID { get; set; }

        public int UserID { get; set; }
    }

    public class InfoEmailValidation
    {
        public string EVToken { get; set; }

        public int UserID { get; set; }

        public string FullName { get; set; }
    }
}
