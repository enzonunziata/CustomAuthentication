using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CustomAuthentication.Part2
{
    public class MyCookieHandler : SignInAuthenticationHandler<MyCookieOptions>
    {
        public MyCookieHandler(IOptionsMonitor<MyCookieOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }

        /// <summary>
        /// Redirects to the login page.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            await Task.CompletedTask;
            Context.Response.Redirect(Options.LoginPath.ToString());
        }

        /// <summary>
        /// Redirects to the access denied page.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            await Task.CompletedTask;
            Context.Response.Redirect(Options.AccessDeniedPath.ToString());
        }

        /// <summary>
        /// Extracts cookie information and checks the values.
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            await Task.CompletedTask;
            if (!Request.Cookies.ContainsKey(Options.CookieName)) return AuthenticateResult.NoResult();

            // extract the authentication ticket from the cookie
            var cookieValue = Request.Cookies[Options.CookieName];
            var ticket = Options.TicketDataFormat.Unprotect(cookieValue);

            if (ticket == null)
            {
                return AuthenticateResult.Fail("Invalid content");
            }

            if (Options.CheckUserAgent)
            {
                var userAgent = Request.Headers["User-Agent"].ToString();
                var claim = ticket.Principal.Claims.Where(x => x.Type == "UserAgent").FirstOrDefault();
                if (claim == null || claim.Value != userAgent)
                {
                    return AuthenticateResult.Fail("Invalid user-agent");
                }
            }

            if (Options.CheckIpAddress)
            {
                var ipAddress = Request.HttpContext.Connection.RemoteIpAddress!.ToString();
                var claim = ticket.Principal.Claims.Where(x => x.Type == "IpAddress").FirstOrDefault();
                if (claim == null || claim.Value != ipAddress)
                {
                    return AuthenticateResult.Fail("Invalid ip-address");
                }
            }

            return AuthenticateResult.Success(ticket);
        }

        /// <summary>
        /// Creates the cookie.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
        {
            await Task.CompletedTask;

            ClaimsIdentity claimsIdentity = (ClaimsIdentity)user.Identity!;
            if (Options.CheckIpAddress && !user.HasClaim(claim => claim.Type == "IpAddress"))
            {
                var ipAddress = Context.Connection.RemoteIpAddress!.ToString();
                claimsIdentity.AddClaim(new Claim("IpAddress", ipAddress));
            }

            if (Options.CheckUserAgent && !user.HasClaim(claim => claim.Type == "UserAgent"))
            {
                var userAgent = Context.Request.Headers["User-Agent"].ToString();
                claimsIdentity.AddClaim(new Claim("UserAgent", userAgent));
            }

            user.AddIdentity(claimsIdentity);

            var ticket = new AuthenticationTicket(user, MyCookieDefaults.AuthenticationScheme);
            string cookieValue = Options.TicketDataFormat.Protect(ticket);

            CookieOptions options = new()
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.Add(Options.ExpireTimeSpan),
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append(Options.CookieName, cookieValue, options);
        }

        /// <summary>
        /// Removes the cookie.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleSignOutAsync(AuthenticationProperties? properties)
        {
            await Task.CompletedTask;
            Response.Cookies.Delete(Options.CookieName);
        }
    }
}
