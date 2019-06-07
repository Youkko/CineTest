using CineTestApi.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace CineTestApi.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        [AllowAnonymous]
        [HttpPost]
        public object Post([FromBody]User user, [FromServices]SigningConfigurations signingConfigurations, [FromServices]TokenConfigurations tokenConfigurations)
        {
            try
            {
                bool validCredentials = false;
                if (user != null && !String.IsNullOrWhiteSpace(user.Key))
                {
                    // Token's validation "Mock". Here, we should check if the user exists
                    // in DB, if his token is valid, etc. But, for this test's purpose, we'll
                    // use a fixed key instead.
                    validCredentials = user.Key == "1f54bd990f1cdfb230adb312546d765d";
                }

                if (validCredentials)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(user.Key, "Login"),
                        new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Key)
                        }
                    );

                    DateTime creationDate = DateTime.Now;
                    DateTime expirationDate = creationDate + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                    {
                        Issuer = tokenConfigurations.Issuer,
                        Audience = tokenConfigurations.Audience,
                        SigningCredentials = signingConfigurations.SigningCredentials,
                        Subject = identity,
                        NotBefore = creationDate,
                        Expires = expirationDate
                    });
                    var token = handler.WriteToken(securityToken);

                    return new
                    {
                        authenticated = true,
                        message = "OK",
                        due = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        token
                    };
                }
                else
                {
                    return new
                    {
                        authenticated = false,
                        message = "Authentication Failure!",
                        due = "",
                        token = ""
                    };
                }
            }
            catch (Exception e)
            {
                return new
                {
                    authenticated = false,
                    message = "[Exception] Authentication Failure!",
                    trace = e.Message,
                    due = "",
                    token = ""
                };
            }
        }
    }
}