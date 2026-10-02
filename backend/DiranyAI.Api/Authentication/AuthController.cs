using DiranyAI.Api.Authentication.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Antiforgery;

namespace DiranyAI.Api.Authentication;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthOptions _authOptions;
    private readonly IAntiforgery _antiforgery;

    public AuthController(IOptions<AuthOptions> authOptions, IAntiforgery antiforgery)
    {
        _authOptions = authOptions.Value;
        _antiforgery = antiforgery;
    }
    [AllowAnonymous]
    [SkipAntiforgery]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (request.Username != _authOptions.Username)
        {
            return Unauthorized();
        }

        var hasher = new PasswordHasher<object>();

        var result = hasher.VerifyHashedPassword(
            null!,
            _authOptions.PasswordHash,
            request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }

        var claims = new List<Claim>
          {
              new(ClaimTypes.Name, _authOptions.Username)
           };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties);

        return Ok();
    }
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            username = User.Identity!.Name
        });
    }
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return NoContent();
    }
    [Authorize]
    [HttpGet("csrf-token")]
    public IActionResult GetCsrfToken()
    {
        var tokens = _antiforgery.GetAndStoreTokens(HttpContext);

        return Ok(new
        {
            token = tokens.RequestToken
        });
    }
}