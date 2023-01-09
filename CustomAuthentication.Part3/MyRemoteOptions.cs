using Microsoft.AspNetCore.Authentication;

namespace CustomAuthentication.Part3
{
    public class MyRemoteOptions : RemoteAuthenticationOptions
    {
        public string AuthorizationEndpoint { get; set; } = string.Empty;
    }
}
