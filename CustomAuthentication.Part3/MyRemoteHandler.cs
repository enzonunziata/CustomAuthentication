using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CustomAuthentication.Part3
{
    public class MyRemoteHandler : RemoteAuthenticationHandler<MyRemoteOptions>
    {
        public MyRemoteHandler(IOptionsMonitor<MyRemoteOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            await Task.CompletedTask;
            Response.Redirect(string.Format("{0}?callbackPath={1}", Options.AuthorizationEndpoint, Options.CallbackPath));
        }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            await Task.CompletedTask;
            string name = Request.Query["name"];

            if (string.IsNullOrWhiteSpace(name))
            {
                return HandleRequestResult.NoResult();
            }

            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, name)
            }, MyRemoteDefaults.AuthenticationScheme);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var ticket = new AuthenticationTicket(claimsPrincipal, MyRemoteDefaults.AuthenticationScheme);
            return HandleRequestResult.Success(ticket);
        }
    }
}
