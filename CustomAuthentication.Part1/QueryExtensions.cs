using Microsoft.AspNetCore.Authentication;

namespace CustomAuthentication.Part1
{
    public static class QueryExtensions
    {
        public static AuthenticationBuilder AddQuery(this AuthenticationBuilder builder)
        {
            return builder.AddQuery(QueryHandler.SchemeName);
        }

        public static AuthenticationBuilder AddQuery(this AuthenticationBuilder builder, string authenticationScheme)
        {
            return builder.AddQuery(authenticationScheme, null!);
        }

        public static AuthenticationBuilder AddQuery(this AuthenticationBuilder builder, string authenticationScheme, Action<QueryOptions> configureOptions)
        {
            return builder.AddQuery(authenticationScheme, null, configureOptions);
        }

        public static AuthenticationBuilder AddQuery(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<QueryOptions> configureOptions)
        {
            return builder.AddScheme<QueryOptions, QueryHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
