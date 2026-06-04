using System.ComponentModel.DataAnnotations;

namespace CarDealership.Models;

public class Brand : BaseEntity
{
    [Required(ErrorMessage = "Numele mărcii este obligatoriu.")]
    [MinLength(2, ErrorMessage = "Numele mărcii trebuie să aibă minimum 2 caractere.")]
    public string Name { get; set; } = string.Empty;

    public List<Car> Cars { get; set; } = [];
}