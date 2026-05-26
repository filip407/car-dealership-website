using System.ComponentModel.DataAnnotations;

namespace CarDealership.DTOs;

public class CreateFeatureDto
{
    [Required(ErrorMessage = "Numele dotarii este obligatoriu.")]
    [MaxLength(100, ErrorMessage = "Numele dotarii nu poate depasi 100 de caractere.")]
    public string Name { get; set; } = string.Empty;
}
