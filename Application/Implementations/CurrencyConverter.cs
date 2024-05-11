using Application.Interfaces;
using Application.Models;

namespace Application.Implementations;

public class CurrencyConverter : ICurrencyConverter
{
    private static readonly Dictionary<string, Dictionary<string, double>> ExchangeRates = new();

    public void UpdateConfiguration(List<ConfigurationEntry>configurationEntries)
    {
        foreach (var edge in configurationEntries)
        {
            if (edge.FromCurrency != null && !ExchangeRates.ContainsKey(edge.FromCurrency))
            {
                ExchangeRates[edge.FromCurrency] = new Dictionary<string, double>();
            }

            if (edge is not {FromCurrency: not null, ToCurrency: not null}) continue;
            
            ExchangeRates[edge.FromCurrency][edge.ToCurrency] = edge.RateValue;

            // Add the opposite exchange rate as well
            if (!ExchangeRates.TryGetValue(edge.ToCurrency, out var value))
            {
                value = new Dictionary<string, double>();
                ExchangeRates[edge.ToCurrency] = value;
            }

            value[edge.FromCurrency] = 1 / edge.RateValue;
        }
    }

    public void ClearConfiguration()
    {
        ExchangeRates.Clear();
    }

    public double Convert(string? fromCurrency, string? toCurrency, double amount)
    {
        if (fromCurrency == toCurrency)
        {
            return amount;
        }
        if (toCurrency != null && fromCurrency != null && (!ExchangeRates.ContainsKey(fromCurrency) || !ExchangeRates.ContainsKey(toCurrency)))
        {
            Console.WriteLine("Exchange rate not available for one or both currencies.");
            return -1;
        }

        var distances = new Dictionary<string, double>();
        foreach (var currency in ExchangeRates.Keys)
        {
            distances[currency] = currency == fromCurrency ? amount : double.MaxValue;
        }

        var visitedCurrencies = new HashSet<string?>();

        while (visitedCurrencies.Count < ExchangeRates.Count)
        {
            var currentCurrency = GetMinDistanceCurrency(distances, visitedCurrencies);
            visitedCurrencies.Add(currentCurrency);

            if (currentCurrency == null) continue;
            foreach (var neighbor in ExchangeRates[currentCurrency])
            {
                if (visitedCurrencies.Contains(neighbor.Key)) continue;
                var newDistance = distances[currentCurrency] * neighbor.Value;
                if (newDistance < distances[neighbor.Key])
                {
                    distances[neighbor.Key] = newDistance;
                }
            }
        }

        return toCurrency != null && distances.TryGetValue(toCurrency, out var value) ? Math.Round(value, 2,MidpointRounding.ToZero)  : -1;
    }
    
    private static string? GetMinDistanceCurrency(Dictionary<string, double> distances, HashSet<string> visitedCurrencies)
    {
        var minDistance = double.MaxValue;
        string? minCurrency = null;

        foreach (var distance in distances.Where(kvp => !visitedCurrencies.Contains(kvp.Key) && kvp.Value < minDistance))
        {
            minDistance = distance.Value;
            minCurrency = distance.Key;
        }

        return minCurrency;
    }
}