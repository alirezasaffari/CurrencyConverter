using Application.Models;

namespace Application.Interfaces;

public interface ICurrencyConverter
{
    /// <summary>
    /// Clears any prior configuration.
    /// </summary>
    void ClearConfiguration();
    /// <summary>
    /// Updates the configuration. Rates are inserted or replaced internally.
    /// </summary>
    void UpdateConfiguration(List<ConfigurationEntry> configurationEntries);

    /// <summary>
    /// Converts the specified amount to the desired currency.
    /// </summary>
    double Convert(string? fromCurrency, string? toCurrency, double amount);
}