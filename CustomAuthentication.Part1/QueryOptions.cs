using Microsoft.AspNetCore.Authentication;

namespace CustomAuthentication.Part1
{
    public class QueryOptions : AuthenticationSchemeOptions
    {
        public string KeyName { get; set; } = string.Empty;
    }
}
