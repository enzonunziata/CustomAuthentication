using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CustomAuthentication.Part1
{
    public class QueryHandler : AuthenticationHandler<QueryOptions>
    {
        public const string SchemeName = "Query";

        public QueryHandler(IOptionsMonitor<QueryOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            await Task.CompletedTask;

            if(string.IsNullOrWhiteSpace(Options.KeyName))
            {
                throw new ArgumentException("Parameter 'KeyName' cannot be empty.");
            }

            if (!Request.Query.ContainsKey(Options.KeyName))
            {
                return AuthenticateResult.NoResult();
            }

            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, Request.Query[Options.KeyName])
            }, SchemeName);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var ticket = new AuthenticationTicket(claimsPrincipal, SchemeName);

            return AuthenticateResult.Success(ticket);
        }
    }
}
