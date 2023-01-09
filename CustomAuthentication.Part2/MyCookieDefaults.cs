namespace CustomAuthentication.Part2
{
    public static class MyCookieDefaults
    {
        public static readonly string AuthenticationScheme = "MyCookie";
        public static readonly PathString LoginPath = new ("/account/login");
        public static readonly PathString AccessDeniedPath = new ("/account/accessdenied");
        public static readonly TimeSpan ExpireTimeSpan = TimeSpan.FromDays(1);
        public static readonly string CookieName = "MyCookieAuth";
    }
}
