using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Paintolog.Models;
using RestSharp;

namespace Paintolog.Controllers;

public class UsersController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;

    public UsersController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet]
    public ActionResult ResetPassword(string token)
    {
        ViewBag.ResultMessage = string.Empty;
        return View(new ChangePasswordViewModel()
        {
            Token = token
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ResetPassword(ChangePasswordViewModel model)
    {
        ViewBag.ResultMessage = string.Empty;
        string api = _configuration["ResetPasswordApiUrl"]!.ToString();
        var client = new RestClient(api);
        var request = new RestRequest("", Method.Post);

        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("accept", "text/plain");

        var body = new
        {
            password = model.NewPassword,
            token = model.Token
        };
        request.AddJsonBody(body);
        try
        {
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var resetPasswordResponse = JsonConvert.DeserializeObject<ResetPasswordResponse>(response.Content!);
                if (resetPasswordResponse!.isSuccess)
                {
                    ViewBag.ResultMessage = "Your password has been successfully changed.";
                }
                else
                {
                    ViewBag.ResultMessage = "Failed to change password.";
                }
                return View(model);
            }
            else
            {
                ViewBag.ResultMessage = "Failed to change password.";
                return View(model);
            }
        }
        catch
        {
            ViewBag.ResultMessage = "Failed to change password.";
            return View(model);
        }
    }
}
