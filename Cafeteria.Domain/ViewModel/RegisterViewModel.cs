using System.ComponentModel.DataAnnotations;

namespace Cafeteria.Domain.ViewModel;

public class RegisterViewModel
{
    [Required(ErrorMessage = "El campo Email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El campo Email no es una dirección de correo electrónico válida.")]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; }


    [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
    [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }


    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")]
    [Compare("Password", ErrorMessage = "La contraseña y la confirmación de contraseña no coinciden.")]
    public string ConfirmPassword { get; set; }


    [Required(ErrorMessage = "Seleccione un rol.")]
    [Display(Name = "Rol")]
    public UserRole Role { get; set; }
}

public enum UserRole
{
    User,
    Supervisor
}

