using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace aspnetapp.TokenProvider
{
    public class JwtTokenProvider : ITokenProvider
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        private readonly byte[] _secretKeyBytes;

        private readonly JwtOptions _jwtOptions;

        private const string TokenType = "Bearer";

        public JwtTokenProvider(IOptions<JwtOptions> options)
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _jwtOptions = options.Value;
            _secretKeyBytes = Encoding.ASCII.GetBytes(_jwtOptions.SigningKey);
        }

        public AuthToken GenerateToken(SignInUser user)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.Expires);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                                             {
                                                 new Claim(ClaimTypes.NameIdentifier, user.Id),
                                                 new Claim(ClaimTypes.Name, user.NickName),
                                                 new Claim(AppClaimTypes.WeChatOpenId, user.WeChatOpenId),
                                                 new Claim(AppClaimTypes.WeChatSessionKey, user.WeChatSessionKey),
                                                 new Claim(JwtRegisteredClaimNames.Aud, _jwtOptions.Audience),
                                                 new Claim(JwtRegisteredClaimNames.Iss, _jwtOptions.Issuer)
                                             }),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            var authToken = new AuthToken
                            {
                                TokenType   = TokenType,
                                AccessToken = _jwtSecurityTokenHandler.WriteToken(token),
                                ExpiresAt   = token.ValidTo
                            };

            return authToken;
        }
    }
}
