using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TransactionWebApp.Models;
using System.Text.RegularExpressions;

namespace TransactionWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;

    public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult TransactionForm()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string? message)
    {
        var errorModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };

        if (!string.IsNullOrEmpty(message))
        {
            // You can add an additional property in the view model to pass the error message, if desired
            // For now, just handle it in a way that fits your needs
            ViewData["ErrorMessage"] = message;
        }

        return View(errorModel);
    }


    public async Task<IActionResult> TransactionForm(string industry)
    {
        ViewBag.Industry = industry;
        string url = $"http://localhost:9798/api/CustomerFields/{industry}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.ResponseCode = "400";
            ViewBag.Message = "Industry not found.";
            return View("TransactionForm");
        }

        var fieldsJson = await response.Content.ReadAsStringAsync();
        var customerField = JsonConvert.DeserializeObject<CustomerField>(fieldsJson);

        if (customerField?.ResponseDetails != null)
        {
            ViewBag.Fields = customerField.ResponseDetails;
            ViewBag.ResponseCode = customerField.ResponseCode; // Add ResponseCode to ViewBag
            ViewBag.Message = customerField.ResponseMsg;
        }
        else
        {
            ViewBag.Fields = null;
            ViewBag.ResponseCode = "NoData"; // Custom code if no fields are available
            ViewBag.Message = customerField?.ResponseMsg ?? "No details available for this industry.";
        }

        return View("TransactionForm");
    }

    [HttpPost]
    public async Task<IActionResult> GetFieldsByAccountNumber(string accountNumber)
    {

        if (string.IsNullOrWhiteSpace(accountNumber) || !Regex.IsMatch(accountNumber, @"^\d{10}$"))
        {
            ViewBag.ResponseCode = "400";
            ViewBag.Message = "Account number must be exactly 10 digits long and numeric.";
            return View("TransactionForm");
        }
        ViewBag.Industry = accountNumber;
        string url = $"http://localhost:9798/api/CustomerFields/{accountNumber}";
        var response = await _httpClient.PostAsync(url, null);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.ResponseCode = "400";
            ViewBag.Message = "Account not found.";
            return View("TransactionForm");
        }

        var fieldsJson = await response.Content.ReadAsStringAsync();
        var customerField = JsonConvert.DeserializeObject<CustomerField>(fieldsJson);

        if (customerField?.ResponseDetails != null)
        {
            ViewBag.Fields = customerField.ResponseDetails;
            ViewBag.ResponseCode = customerField.ResponseCode; // Add ResponseCode to ViewBag
            ViewBag.Message = customerField.ResponseMsg;
        }
        else
        {
            ViewBag.Fields = null;
            ViewBag.ResponseCode = "NoData"; // Custom code if no fields are available
            ViewBag.Message = customerField?.ResponseMsg ?? "No details available for this industry.";
        }

        return View("TransactionForm");
    }




}
