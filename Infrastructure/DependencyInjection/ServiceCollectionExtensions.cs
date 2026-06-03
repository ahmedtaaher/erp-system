using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Common.Interfaces;
using Infrastructure.Auth;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddHttpContextAccessor();
      services.AddScoped<ITenantProvider, TenantProvider>();
      services.AddScoped<IJwtProvider, JwtProvider>();
      services.AddScoped<IPasswordHasher, PasswordHasher>();
      services.AddScoped<ICurrentUser, CurrentUserService>();
      services.AddDbContext<ERPDbContext>(
        options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
      );
      services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

      return services;
    }
  }
}