using Microsoft.AspNetCore.Authentication.Cookies;

namespace CustomAuthentication.Part3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; //standard cookie auth
                options.DefaultChallengeScheme = MyRemoteDefaults.AuthenticationScheme; //custom remote auth
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddScheme<MyRemoteOptions, MyRemoteHandler>(MyRemoteDefaults.AuthenticationScheme, options =>
            {
                options.AuthorizationEndpoint = "https://localhost:7207/Home/Authorize"; // remote endpoint
                options.CallbackPath = "/Home/Callback"; // local callback endpoint
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}