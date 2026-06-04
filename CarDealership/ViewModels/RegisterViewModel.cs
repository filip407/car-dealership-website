using System.ComponentModel.DataAnnotations;

namespace CarDealership.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Numele complet este obligatoriu.")]
    [Display(Name = "Nume Complet")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email-ul este obligatoriu.")]
    [EmailAddress(ErrorMessage = "Adresa de email nu este validă.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola este obligatorie.")]
    [MinLength(6, ErrorMessage = "Parola trebuie să aibă minim 6 caractere.")]
    [DataType(DataType.Password)]
    [Display(Name = "Parolă")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmarea parolei este obligatorie.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Parolele nu coincid.")]
    [Display(Name = "Confirmare Parolă")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
