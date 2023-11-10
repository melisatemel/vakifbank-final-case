using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrderManagementSystem.Base.Token;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Operation.Cqrs;
using OrderManagementSystem.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Base;

namespace OrderManagementSystem.Operation.Command;

public class TokenCommandHandler :
    IRequestHandler<CreateTokenCommand, ApiResponse<TokenResponse>>

{
    private readonly OmsDbContext dbContext;
    private readonly JwtConfig jwtConfig;

    public TokenCommandHandler(OmsDbContext dbContext, IOptionsMonitor<JwtConfig> jwtConfig)
    {
        this.dbContext = dbContext;
        this.jwtConfig = jwtConfig.CurrentValue;
    }


    public async Task<ApiResponse<TokenResponse>> Handle(CreateTokenCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<User>().FirstOrDefaultAsync(x => x.Email == request.Model.Email, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse<TokenResponse>("Invalid user informations");
        }

        var md5 = Md5.Create(request.Model.Password.ToUpper());
        if (entity.Password != md5)
        {
            entity.LastActivityDate = DateTime.UtcNow;
            entity.PasswordRetryCount++;
            await dbContext.SaveChangesAsync(cancellationToken);

            return new ApiResponse<TokenResponse>(md5);
        }

        if (!entity.IsActive)
        {
            return new ApiResponse<TokenResponse>("Invalid user!");
        }

        string token = Token(entity);
        TokenResponse tokenResponse = new()
        {
            Token = token,
            ExpireDate = DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration),
            UserId = entity.UserId,
            Email = entity.Email
        };

        return new ApiResponse<TokenResponse>(tokenResponse);
    }

    private string Token(User user)
    {
        Claim[] claims = GetClaims(user);
        var secret = Encoding.ASCII.GetBytes(jwtConfig.Secret);

        var jwtToken = new JwtSecurityToken(
            jwtConfig.Issuer,
            jwtConfig.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
        );

        string accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return accessToken;
    }


    private Claim[] GetClaims(User user)
    {
        var claims = new[]
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("UserId", user.UserId.ToString()),
            new Claim("Role", user.Role),
            new Claim("Email", user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("FullName", $"{user.FirstName} {user.LastName}")
        };

        return claims;
    }
}