using System.ComponentModel.DataAnnotations;

namespace Cafeteria.Domain.ViewModel;

public class LoginViewModel
{
    [Required(ErrorMessage = "El campo Email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El campo Email no es una dirección de correo electrónico válida.")]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; }

    [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }

    [Display(Name = "Recordarme")]
    public bool RememberMe { get; set; }
}


