using Application.Implementations;
using Application.Models;

namespace TestProject;

public class ConverterTest
{
    private readonly CurrencyConverter _converter = new CurrencyConverter();

    private readonly ConfigurationModel _configurationModel = new([
        new ConfigurationEntry {FromCurrency = "usd", ToCurrency = "cad", RateValue = 1.34},
        new ConfigurationEntry {FromCurrency = "cad", ToCurrency = "gbp", RateValue = 0.58},
        new ConfigurationEntry {FromCurrency = "usd", ToCurrency = "eur", RateValue = 0.86}
    ]);
    
    [Theory]
    [InlineData("usd", "cad", 1000,1340)]
    [InlineData("cad", "gbp", 1000,580)]
    [InlineData("usd", "eur", 1000,860)]
    [InlineData("cad","usd", 1000,746.26)]
    [InlineData("gbp","cad", 1000,1724.13)]
    [InlineData( "eur","usd", 1000,1162.79)]
    [InlineData( "usd","gbp", 1000,777.2)]
    [InlineData( "cad","eur", 1000,641.79)]
    [InlineData( "jpy","eur", 1000,-1)]
    
    public void Convert_currency_should_return_result(string? fromCurrency, string? toCurrency,double amount,double expected)
    {
        _converter.UpdateConfiguration(_configurationModel.Configurations);
        var result = _converter.Convert(fromCurrency,toCurrency,amount);

        Assert.Equal(expected, result);
    }
}
