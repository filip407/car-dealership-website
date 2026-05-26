using System.ComponentModel.DataAnnotations;

namespace CarDealership.DTOs;

public class UpdateCarDto
{
    [Required(ErrorMessage = "Marca este obligatorie.")]
    public int BrandId { get; set; }

    [Required(ErrorMessage = "Modelul este obligatoriu.")]
    [MinLength(2, ErrorMessage = "Modelul trebuie sa aiba minimum 2 caractere.")]
    public string ModelName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Anul este obligatoriu.")]
    [Range(1900, 2027, ErrorMessage = "Anul trebuie sa fie intre 1900 si 2027.")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Pretul este obligatoriu.")]
    [Range(1, 10_000_000, ErrorMessage = "Pretul trebuie sa fie intre 1 si 10.000.000.")]
    public decimal Price { get; set; }

    public int[] SelectedFeatureIds { get; set; } = Array.Empty<int>();
}
