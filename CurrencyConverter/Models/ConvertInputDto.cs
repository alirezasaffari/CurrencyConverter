using System.ComponentModel.DataAnnotations;

namespace CurrencyConverter.Models;

public class ConvertInputDto
{
    [Required(ErrorMessage = "From Currency is required")]
    public string? FromCurrency { get; init; }

    [Required(ErrorMessage = "To Currency is required")]
    public string? ToCurrency { get; init; }
    
    [Required(ErrorMessage = "Amount is required")]
    public double Amount { get; init; } 
}