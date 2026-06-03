using Application.Common.Models;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,

      ValidIssuer = jwtSettings!.Issuer,
      ValidAudience = jwtSettings.Audience,
      IssuerSigningKey = new SymmetricSecurityKey(
      Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
  });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}
app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();