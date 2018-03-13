using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using WebApiAuth.Infra;
using WebApiAuth.Models;

namespace WebApiAuth.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Authenticate([FromBody] LoginRequest login)
        {
            var loginResponse = new LoginResponse { };
            var loginrequest = new LoginRequest { };
            loginrequest.Username = login.Username.ToLower();
            loginrequest.Password = login.Password;

            IHttpActionResult response;
            var responseMsg = new HttpResponseMessage();
            var isUsernamePasswordValid = false;

            if (login != null)
                isUsernamePasswordValid = loginrequest.Password == "admin" ? true : false;
            // if credentials are valid
            if (isUsernamePasswordValid)
            {
                var token = CreateToken(loginrequest.Username);
                //return the token
                return Ok(token);
            }
            else
            {
                // if credentials are not valid send unauthorized status code in response
                loginResponse.ResponseMsg.StatusCode = HttpStatusCode.Unauthorized;
                response = ResponseMessage(loginResponse.ResponseMsg);
                return response;
            }
        }

        private string CreateToken(string username)
        {
            //Set issued at date
            var issuedAt = DateTime.UtcNow;

            //set the time when it expires
            var expires = DateTime.UtcNow.AddDays(7);

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            });

            var sec = SecurityTokenGenerator.SecretKey;
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


            //create the jwt
            var token = tokenHandler.CreateJwtSecurityToken(
                    issuer: "http://localhost:55713", 
                    audience: "http://localhost:55713",
                    subject: claimsIdentity, 
                    notBefore: issuedAt, 
                    expires: expires, 
                    signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
