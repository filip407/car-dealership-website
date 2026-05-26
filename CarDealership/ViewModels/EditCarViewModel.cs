using System.ComponentModel.DataAnnotations;
using CarDealership.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarDealership.ViewModels;

public class EditCarViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Marca este obligatorie")]
    [Display(Name = "Marcă Auto")]
    public int BrandId { get; set; }

    [Required(ErrorMessage = "Modelul este obligatoriu")]
    [MinLength(2, ErrorMessage = "Modelul trebuie să aibă minim 2 caractere")]
    [Display(Name = "Model")]
    public string ModelName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Anul este obligatoriu")]
    [Range(1900, 2027, ErrorMessage = "Anul trebuie să fie între 1900 și 2027")]
    [Display(Name = "An Fabricație")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Prețul este obligatoriu")]
    [Range(1, 10000000, ErrorMessage = "Prețul trebuie să fie pozitiv")]
    [Display(Name = "Preț (EUR)")]
    public decimal Price { get; set; }

    public string? ExistingImagePath { get; set; }

    [Display(Name = "Înlocuiește Imaginea (Opțional)")]
    public IFormFile? ImageFile { get; set; }

    public int[] SelectedFeatureIds { get; set; } = Array.Empty<int>();

    public List<SelectListItem> Brands { get; set; } = new();
    public List<Feature> AllFeatures { get; set; } = new();
}
