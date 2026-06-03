using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Login
{
  public class LoginHandler : IRequestHandler<LoginCommand, string>
  {
    private readonly IApplicationDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtProvider _jwt;

    public LoginHandler(IApplicationDbContext db, IPasswordHasher hasher, IJwtProvider jwt)
    {
      _db = db;
      _hasher = hasher;
      _jwt = jwt;
    }
    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
      var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == request.Email.ToLower() && x.TenantId == request.TenantId, cancellationToken);

      if (user == null)
        throw new Exception("Invalid credentials");

      var isValid = _hasher.Verify(request.Password, user.PasswordHash);

      if (!isValid)
        throw new Exception("Invalid credentials");

      return _jwt.GenerateToken(user);
    }
  }
}