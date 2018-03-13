using System.Net;
using System.Net.Http;

namespace WebApiAuth.Models
{
    public class LoginResponse
    {
        public LoginResponse()
        {

            Token = "";
            ResponseMsg = new HttpResponseMessage {StatusCode = HttpStatusCode.Unauthorized};
        }

        public string Token { get; }
        public HttpResponseMessage ResponseMsg { get; }

    }
}