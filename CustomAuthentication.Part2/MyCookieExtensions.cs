using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace CustomAuthentication.Part2
{
    public static class MyCookieExtensions
    {
        public static AuthenticationBuilder AddMyCookie(this AuthenticationBuilder builder)
        {
            return builder.AddMyCookie(MyCookieDefaults.AuthenticationScheme);
        }

        public static AuthenticationBuilder AddMyCookie(this AuthenticationBuilder builder, string authenticationScheme)
        {
            return builder.AddMyCookie(authenticationScheme, null!);
        }

        public static AuthenticationBuilder AddMyCookie(this AuthenticationBuilder builder, string authenticationScheme, Action<MyCookieOptions> configureOptions)
        {
            return builder.AddMyCookie(authenticationScheme, null, configureOptions);
        }

        public static AuthenticationBuilder AddMyCookie(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<MyCookieOptions> configureOptions)
        {
            return builder.AddScheme<MyCookieOptions, MyCookieHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
