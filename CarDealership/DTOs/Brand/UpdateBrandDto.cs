using System.ComponentModel.DataAnnotations;

namespace CarDealership.DTOs;

public class UpdateBrandDto
{
    [Required(ErrorMessage = "Numele marcii este obligatoriu.")]
    [MinLength(2, ErrorMessage = "Numele marcii trebuie sa aiba minimum 2 caractere.")]
    public string Name { get; set; } = string.Empty;
}
