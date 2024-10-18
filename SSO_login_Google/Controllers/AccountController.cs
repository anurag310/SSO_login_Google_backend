using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        var redirectUrl = Url.Action("GoogleResponse", "Account");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    //[HttpGet("GoogleResponse")]
    //public async Task<IActionResult> GoogleResponse()
    //{
    //    var result = await HttpContext.AuthenticateAsync();

    //    if (!result.Succeeded)
    //    {
    //        return Unauthorized(); // Return unauthorized if authentication fails
    //    }

    //    // Extract user information from claims
    //    var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
    //    var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

    //    // Simulate user sign-in or processing (you can return user info for testing)
    //    return Ok(new { Email = email, Name = name });
    //}
    [HttpGet("GoogleResponse")]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync();

        if (!result.Succeeded)
        {
            return Unauthorized(); // Return unauthorized if authentication fails
        }

        // Extract user information from claims
        var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
        var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
        var items = result.Properties.Items;
        // Extract Google access token
        var token = result.Properties.GetTokenValue("access_token");

        // Return token and user info to the client
        return Ok(new { Token = token, Email = email, Name = name });
    }

}
