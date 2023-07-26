using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Google_Login_Dot_Net_7_0.Pages;
[ValidateAntiForgeryToken]

public class SignInModel : PageModel
{
    private readonly ILogger<SignInModel> _logger;
    public string Issuer,
                OriginalIssuer,
                Type,
                Value;
    public SignInModel(ILogger<SignInModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }

    public IActionResult OnPostGoogleLogin()
    {
        var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        var claims = result.Principal.Identities.FirstOrDefault()
            .Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });
        Issuer = claims.FirstOrDefault().Issuer;
        OriginalIssuer = claims.FirstOrDefault().OriginalIssuer;
        Type = claims.FirstOrDefault().Type;
        Value = claims.FirstOrDefault().Value;

        return new JsonResult(claims);
    }
}
