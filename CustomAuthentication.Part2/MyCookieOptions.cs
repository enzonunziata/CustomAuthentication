using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;

namespace CustomAuthentication.Part2
{
    public class MyCookieOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// Used inside HandleChallengeAsync to redirect the user to the login page.
        /// </summary>
        public PathString LoginPath { get; set; } = MyCookieDefaults.LoginPath;

        /// <summary>
        /// Used inside HandleForbidenAsync to redirect the user to the access denied page.
        /// </summary>
        public PathString AccessDeniedPath { get; set; } = MyCookieDefaults.AccessDeniedPath;

        /// <summary>
        /// How much time the cookie will be valid.
        /// </summary>
        public TimeSpan ExpireTimeSpan { get; set; } = MyCookieDefaults.ExpireTimeSpan;

        /// <summary>
        /// Name of the authentication cookie.
        /// </summary>
        public string CookieName { get; set; } = MyCookieDefaults.CookieName;

        /// <summary>
        /// Property added over the standard behavior.
        /// A claim named "IpAddress" is added to the identity. If this property is true, then 
        /// the requester's ip-address must match.
        /// </summary>
        public bool CheckIpAddress { get; set; } = true;

        /// <summary>
        /// Property added over the standard behavior.
        /// A claim named "UserAgent" is added to the identity. If this property is true, then 
        /// the requester's user-agent must match.
        /// </summary>
        public bool CheckUserAgent { get; set; } = true;

        /// <summary>
        /// This property is used to protect/unprotect data inside the cookie.
        /// </summary>
        internal ISecureDataFormat<AuthenticationTicket> TicketDataFormat { get; }

        public MyCookieOptions()
        {
            var serializer = new TicketSerializer();
            var dataProtector = DataProtectionProvider.Create(typeof(MyCookieHandler).FullName!).CreateProtector("ticket");

            TicketDataFormat = new SecureDataFormat<AuthenticationTicket>(serializer, dataProtector);
        }
    }
}
