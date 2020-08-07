using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Shop.Models;
using Microsoft.IdentityModel.Tokens;

namespace Shop.Services
{
    public static class TokenService
    {
        public static string GenerateToken(User user)   //  (gera um token) é token que é chamado de GenerateToken(User user) que receber um user.
        {
            var tokenHandler = new JwtSecurityTokenHandler();   //  quem manipula o token, resposavel por gerar o token
            var key = Encoding.ASCII.GetBytes(Settings.Secret); //  novamente a chave
            var tokenDescriptor = new SecurityTokenDescriptor   //  basicamente a descrição, do que que vai ter dentro do token.
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),  //  data de expiração do token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)  //  ele usa a chave novamente, e gera baseado em um algortimo chamado Sha256
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);  //  cria o token conforme passado no tokenDescriptor
            return tokenHandler.WriteToken(token);  //  ele gera a string do token.
        }
    }
}