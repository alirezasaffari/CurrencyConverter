using System.Diagnostics;
using Application.Implementations;
using Application.Interfaces;
using Application.Models;
using CurrencyConverter.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Controllers;

public class CurrencyController : Controller
{
    private readonly ICurrencyConverter _currencyConverter = new Application.Implementations.CurrencyConverter();
    public IActionResult Index(ConfigurationModel? configurationModel = null)
    {
        return View("Index", configurationModel ?? new ConfigurationModel([]));
    }
    public IActionResult UpdateConfiguration(ConfigurationModel configurationModel)
    {
        if (ModelState.IsValid)
        {
            _currencyConverter.UpdateConfiguration(configurationModel.Configurations);
        }
        ViewBag.UpdateSuccess = true;
        return Index(configurationModel);
    }
    
    public IActionResult ClearConfiguration()
    {
        _currencyConverter.ClearConfiguration();
        ViewBag.IsClearSuccess = true;
        return Index();
    }
    
    public IActionResult ConvertCurrency(ConvertInputDto convertInputDto)
    {
        ViewBag.ConvertedAmount = _currencyConverter.Convert(convertInputDto.FromCurrency,convertInputDto.ToCurrency,convertInputDto.Amount);
        return Converter();
    }
    
    public ActionResult Converter()
    {
        return View("Converter");
    }

   [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}