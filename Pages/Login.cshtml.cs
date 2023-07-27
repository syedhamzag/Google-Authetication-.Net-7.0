using Google_Login_Dot_Net_7_0.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Google_Login_Dot_Net_7_0.Pages;
[ValidateAntiForgeryToken]

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    public LoginModel(ILogger<LoginModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostGoogleLogin()
    {
        //var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse","GoogleResponse") };
        var properties = new AuthenticationProperties { };
        properties.RedirectUri = "/Login?handler=GoogleResponse";
        properties.AllowRefresh = false;
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }
    public async Task<IActionResult> OnGetGoogleResponse()
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
        return new JsonResult(claims);
        //     var content = JsonConvert.SerializeObject(claims);
        //     List<GoogleUser> _User = JsonConvert.DeserializeObject<List<GoogleUser>>(content.ToString());
        // Console.WriteLine(_User.Where(s=>s.Type.EndsWith("name")).FirstOrDefault().Value.ToString());
    }
}
