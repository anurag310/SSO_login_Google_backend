using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://localhost:4200") // Angular app URL
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

// Add Authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use Always in production; use Never for local dev without HTTPS
    options.Cookie.SameSite = SameSiteMode.None; // Must be None for cross-origin requests
    options.LoginPath = "/api/Account/Login"; // Set your login path
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = "83805026008-45i45ul5htnun6jcrsaj6fevfjkgill8.apps.googleusercontent.com"; // Replace with your Google Client ID
    options.ClientSecret = "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"; // Replace with your Google Client Secret
    options.CallbackPath = "/signin-google"; // Path for Google response handling
    options.SaveTokens = true;
}).AddFacebook(FacebookDefaults.AuthenticationScheme, options =>
{
    options.AppId = "3882378972009554"; // Replace with your Facebook App ID
    options.AppSecret = "448df6f6672e5abffa4f635da1abff1c"; // Replace with your Facebook App Secret
    options.CallbackPath = "/signin-facebook"; // Path for Facebook response handling
    options.SaveTokens = true;
}); 

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
