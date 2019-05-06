using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Samples.ClientCredential.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("token")]
        public ActionResult CreateToken()
        {
            var securityKey = "this is our supper security key";
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signingCredentials =
                new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "admin"), 
                new Claim("custom claim", "our custom value")
            };

            var token = new JwtSecurityToken(issuer: "yang", audience: "readers", expires: DateTime.Now.AddDays(1),
                signingCredentials: signingCredentials, claims: claims);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}