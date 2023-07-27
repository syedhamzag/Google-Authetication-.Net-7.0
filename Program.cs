using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var confirguration = builder.Configuration;

builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
// Add services to the container.
builder.Services.AddRazorPages();

services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    //options.LoginPath = "/signin-google"; Default login path needs to change it on Google Console/Credentials/OAuth 2.0 Client IDs
                    options.LoginPath = "/";
                })
                .AddGoogle(options =>
                {
                    var googleAuthNSection =
                    confirguration.GetSection("Authentication:Google");
                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthorization();

// app.MapRazorPages();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.MapRazorPages();

app.Run();
