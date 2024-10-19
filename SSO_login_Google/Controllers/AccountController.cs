using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    // Initiates Google login
    [HttpGet("login")]
    public IActionResult Login()
    {
        var redirectUrl = Url.Action("GoogleResponse", "Account");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    // Initiates Facebook login
    [HttpGet("login/facebook")]
    public IActionResult FacebookLogin()
    {
        var redirectUrl = Url.Action("FacebookResponse", "Account");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, FacebookDefaults.AuthenticationScheme);
    }

    // Handles Google login response
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

        // Extract Google access token
        var token = result.Properties.GetTokenValue("access_token");

        // Return token and user info to the client
        return Ok(new { Token = token, Email = email, Name = name });
    }

    // Handles Facebook login response
    [HttpGet("FacebookResponse")]
    public async Task<IActionResult> FacebookResponse()
    {
        var result = await HttpContext.AuthenticateAsync();

        if (!result.Succeeded)
        {
            return Unauthorized(); // Return unauthorized if authentication fails
        }

        // Extract user information from claims
        var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
        var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

        // Extract Facebook access token
        var token = result.Properties.GetTokenValue("access_token");

        // Return token and user info to the client
        return Ok(new { Token = token, Email = email, Name = name });
    }
}
