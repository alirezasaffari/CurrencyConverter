using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class ConfigurationModel(List<ConfigurationEntry> configurations)
{
    public List<ConfigurationEntry> Configurations { get; init; } = configurations;
}

public class ConfigurationEntry
{
    [Required(ErrorMessage = "From Currency is required")]
    public string? FromCurrency { get; init; }

    [Required(ErrorMessage = "To Currency is required")]
    public string? ToCurrency { get; init; }

    [Required(ErrorMessage = "Rate Value is required")]
    public double RateValue { get; init; }
}